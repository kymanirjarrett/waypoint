#!/usr/bin/env bash
# Apply EF Core migrations to Azure SQL and (first run) create the web app's database user.
# Runs as the signed-in Entra admin (`az login`); the app itself never gets schema rights.
# Opens a firewall rule for this machine's IP only for the duration of the script.
set -euo pipefail

RG=rg-waypoint
SQL=sql-waypoint-06d12e
DB=waypoint
APP=waypoint
CONN="Server=tcp:$SQL.database.windows.net,1433;Database=$DB;Authentication=Active Directory Default;Encrypt=True;"

MYIP=$(curl -s -4 https://api.ipify.org)
# Mobile/carrier networks rotate the public IP between connections, so open the /16 around it.
# Still safe while it is open: the server accepts Entra sign-ins only, and the rule is removed on exit.
START="${MYIP%.*.*}.0.0"; END="${MYIP%.*.*}.255.255"
az sql server firewall-rule create -g "$RG" -s "$SQL" -n temp-migrate --start-ip-address "$START" --end-ip-address "$END" -o none
trap 'az sql server firewall-rule delete -g "$RG" -s "$SQL" -n temp-migrate -o none && echo "Removed temp firewall rule"' EXIT

# A new firewall rule can take a few minutes to apply.
for attempt in 1 2 3 4 5 6; do
  dotnet ef database update --project Waypoint --connection "$CONN" && break
  [ "$attempt" = 6 ] && exit 1
  echo "Retrying in 30s (firewall rule may still be applying)"; sleep 30
done

# Least privilege: read and write data only. No db_ddladmin, because only this script changes the schema.
sqlcmd -S "$SQL.database.windows.net" -d "$DB" --authentication-method ActiveDirectoryAzCli -b -Q "
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'$APP')
    CREATE USER [$APP] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [$APP];
ALTER ROLE db_datawriter ADD MEMBER [$APP];
SELECT r.name AS role_name FROM sys.database_role_members m
JOIN sys.database_principals r ON r.principal_id = m.role_principal_id
JOIN sys.database_principals u ON u.principal_id = m.member_principal_id
WHERE u.name = N'$APP';"
