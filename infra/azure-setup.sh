#!/usr/bin/env bash
# One-time Azure setup for Waypoint. Run step by step; each block is safe to read before running.
# Requires: az CLI signed in (`az login`) to the Azure for Students subscription.
# No passwords or keys are created: the app reaches SQL with its managed identity,
# and GitHub Actions deploys through OIDC federation.
set -euo pipefail

RG=rg-waypoint
LOC=westus                    # student subscription allows only a few regions; westus had F1 quota (centralus had 0)
PLAN=asp-waypoint
APP=waypoint
SQL=sql-waypoint-06d12e       # SQL server names are global, hence the suffix
SQL_LOC=centralus             # westus was not accepting new SQL servers
DB=waypoint
DEPLOY_ID=id-waypoint-deploy
REPO=kymanirjarrett/waypoint

# --- 0. Resource providers (one time per subscription) ---
for ns in Microsoft.Web Microsoft.Sql Microsoft.ManagedIdentity; do
  az provider register --namespace "$ns" --wait
done

# --- 1. Resource group ---
[ "$(az group exists -n "$RG")" = true ] || az group create -n "$RG" -l "$LOC"

# --- 2. App Service: Free F1 on Windows (64-bit supported on F1), .NET 10 ---
# az now defaults plans to Linux; --is-linux false picks Windows.
az appservice plan create -g "$RG" -n "$PLAN" -l "$LOC" --sku F1 --is-linux false
# TenantReuse gives the secure unique default hostname: <app>-<hash>.<region>.azurewebsites.net
az webapp create -g "$RG" -p "$PLAN" -n "$APP" --runtime "dotnet:10" --domain-name-scope TenantReuse
az webapp config set -g "$RG" -n "$APP" --use-32bit-worker-process false --ftps-state Disabled --min-tls-version 1.2 --http20-enabled true
az webapp update -g "$RG" -n "$APP" --https-only true
# Basic-auth publishing (SCM and FTP) off: deploys only through Entra ID.
for kind in scm ftp; do
  az resource update -g "$RG" --namespace Microsoft.Web --resource-type basicPublishingCredentialsPolicies \
    --parent "sites/$APP" -n "$kind" --set properties.allow=false
done

# --- 3. Azure SQL: Entra-only auth, free-offer serverless DB that pauses (never bills) at the free limit ---
ME_ID=$(az ad signed-in-user show --query id -o tsv)
ME_UPN=$(az ad signed-in-user show --query userPrincipalName -o tsv)
az sql server create -g "$RG" -n "$SQL" -l "$SQL_LOC" --enable-ad-only-auth \
  --external-admin-principal-type User --external-admin-name "$ME_UPN" --external-admin-sid "$ME_ID"
az sql db create -g "$RG" -s "$SQL" -n "$DB" -e GeneralPurpose -f Gen5 -c 2 --compute-model Serverless \
  --use-free-limit --free-limit-exhaustion-behavior AutoPause
# "Allow Azure services and resources to access this server"
az sql server firewall-rule create -g "$RG" -s "$SQL" -n AllowAllWindowsAzureIps --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# --- 4. Web app identity + connection string (stored in App Service, never in the repo) ---
az webapp identity assign -g "$RG" -n "$APP"
az webapp config connection-string set -g "$RG" -n "$APP" -t SQLAzure --settings \
  WaypointDb="Server=tcp:$SQL.database.windows.net,1433;Database=$DB;Authentication=Active Directory Default;Encrypt=True;"
az webapp config appsettings set -g "$RG" -n "$APP" --settings ASPNETCORE_ENVIRONMENT=Production

# --- 5. Deploy identity: user-assigned MI trusted for this repo's production environment only ---
az identity create -g "$RG" -n "$DEPLOY_ID" -l "$LOC"
az identity federated-credential create -g "$RG" --identity-name "$DEPLOY_ID" -n github-production \
  --issuer https://token.actions.githubusercontent.com \
  --subject "repo:$REPO:environment:production" --audiences api://AzureADTokenExchange
az role assignment create --role "Website Contributor" \
  --assignee-object-id "$(az identity show -g "$RG" -n "$DEPLOY_ID" --query principalId -o tsv)" \
  --assignee-principal-type ServicePrincipal \
  --scope "$(az webapp show -g "$RG" -n "$APP" --query id -o tsv)"

# --- 6. $5 monthly budget with an email alert at 80% (actual) to the signed-in user ---
SUB=$(az account show --query id -o tsv)
az rest --method put \
  --url "/subscriptions/$SUB/providers/Microsoft.Consumption/budgets/waypoint-5usd?api-version=2023-11-01" \
  --body "{\"properties\":{\"category\":\"Cost\",\"amount\":5,\"timeGrain\":\"Monthly\",
    \"timePeriod\":{\"startDate\":\"$(date +%Y-%m-01)T00:00:00Z\",\"endDate\":\"2028-06-30T00:00:00Z\"},
    \"notifications\":{\"actual80\":{\"enabled\":true,\"operator\":\"GreaterThanOrEqualTo\",\"threshold\":80,
    \"contactEmails\":[\"$ME_UPN\"],\"thresholdType\":\"Actual\"}}}}"

echo "Web app: https://$(az webapp show -g "$RG" -n "$APP" --query defaultHostName -o tsv)"
echo "Deploy identity client id: $(az identity show -g "$RG" -n "$DEPLOY_ID" --query clientId -o tsv)"
