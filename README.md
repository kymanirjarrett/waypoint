# Waypoint

[![CI](https://github.com/kymanirjarrett/waypoint/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/kymanirjarrett/waypoint/actions/workflows/ci.yml)

A travel guide by three friends: places we've been, places we're saving up for, when to go, and what a day there costs.

**Live:** https://waypoint-e0dcgkgub6dpbfdv.westus-01.azurewebsites.net (free tier, so the first visit after a quiet spell can take a few seconds while it wakes up)

## Features

- Destination cards with continent filters, a detail page per destination, and a sortable table of every row in the database
- Each destination marked as visited or on the wish list, with credited photos
- Team pages, plus Gallery and Travel Tips sections
- Full-width, responsive design with WCAG AA contrast and keyboard support

## Tech stack

ASP.NET Core MVC on .NET 10 · Razor with Bootstrap 5 and jQuery · Entity Framework Core 10 on SQL Server · xUnit · GitHub Actions · Azure App Service and Azure SQL

## Architecture

```
GitHub Actions (main) --OIDC--> Azure App Service (Free F1, .NET 10)
                                   |  managed identity, no passwords
                                   v
                                Azure SQL Database (free offer, serverless)
```

- **No stored secrets.** The app signs in to SQL with its managed identity, and deploys sign in to Azure with OpenID Connect. The repo and GitHub hold no passwords or keys.
- **Least privilege.** The app's database user can only read and write data, not change the schema. Migrations run separately as the admin (`infra/azure-migrate.sh`).
- **Never bills.** The free database pauses instead of charging when it reaches its monthly limit, and the app retries while it resumes.
- **Every push to `main` deploys**, then a smoke test checks the key pages return 200.
- **Releases with a migration:** run `infra/azure-migrate.sh` before merging the release into `main`. Deploys do not change the schema, and migrations should stay additive so the running version keeps working.
- **Accepted risk:** the SQL firewall allows Azure services (`0.0.0.0`) because the free App Service plan has no private networking. Sign-in is Entra-only, so reaching the endpoint is not enough to log in.
- `infra/azure-setup.sh` recreates every Azure resource with the Azure CLI. `/healthz` reports whether the app is up without touching the database.

## Run it locally

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

```bash
dotnet run --project Waypoint --launch-profile https   # https://localhost:7085
dotnet test
```

- **Windows:** uses SQL Server LocalDB from `appsettings.json`. The database and seed data are created on first run.
- **macOS:** LocalDB is Windows-only. Run SQL Server in Docker (on Apple Silicon, enable Rosetta in Docker Desktop) and point the app at it with a user secret:

  ```bash
  docker run -d --platform linux/amd64 --name waypoint-sql -e ACCEPT_EULA=Y -e "MSSQL_SA_PASSWORD=<your-password>" -p 14333:1433 mcr.microsoft.com/mssql/server:2022-latest
  dotnet user-secrets set "ConnectionStrings:WaypointDb" "Server=localhost,14333;Database=Waypoint;User Id=sa;Password=<your-password>;TrustServerCertificate=True" --project Waypoint
  ```

Schema changes: `dotnet ef migrations add <Name> --project Waypoint`. Seed data lives in `Waypoint/Data/SeedData.cs`.

## Team

| | GitHub | Built |
|---|---|---|
| Kymani Jarrett | [@kymanirjarrett](https://github.com/kymanirjarrett) | Setup, CI/CD, design system, Azure deployment |
| Riddhi Mahajan | [@riddhuu](https://github.com/riddhuu) | Destinations and the database |
| Evan Schmidt | [@schmidev](https://github.com/schmidev) | Gallery and Travel Tips |

## Credits

Destination photos from [Unsplash](https://unsplash.com), credited in [`wwwroot/images/CREDITS.md`](Waypoint/wwwroot/images/CREDITS.md) and on each page. Fonts: Gloock and Public Sans (SIL Open Font License).
