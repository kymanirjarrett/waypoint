# Waypoint

[![CI](https://github.com/kymanirjarrett/waypoint/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/kymanirjarrett/waypoint/actions/workflows/ci.yml)

Waypoint is a travel destination guide built with ASP.NET Core MVC on .NET 10. It's a team project for IT3047C Web Server Application Development at the University of Cincinnati, documenting our shared hobby: travel.

## Team

| Member | GitHub | Owns |
|---|---|---|
| Kymani Jarrett | [@kymanirjarrett](https://github.com/kymanirjarrett) | Project setup, layout and navigation, About pages, CI |
| Riddhi Mahajan | [@riddhuu](https://github.com/riddhuu) | Destinations (list, detail pages, slug routing) |
| Evan Schmidt | [@schmidev](https://github.com/schmidev) | Gallery and Travel Tips |

Everyone owns their own About page (`Views/About/<Name>.cshtml`).

## Tech stack

- .NET 10, ASP.NET Core MVC (C#)
- Razor views with Bootstrap 5 and jQuery
- Entity Framework Core 10 with SQL Server LocalDB
- xUnit for tests
- GitHub Actions for CI, Dependabot for dependency updates
- Developed in JetBrains Rider

## Routes

All routes use attribute routing (`[Route]` on each action) and lowercase URLs.

| Route | Page | Controller / Action | Status |
|---|---|---|---|
| `/` | Home | `HomeController.Index` | Done |
| `/privacy` | Privacy | `HomeController.Privacy` | Done |
| `/about` | About Us (team overview) | `AboutController.Index` | Done |
| `/about/kymani` | Kymani's page | `AboutController.Kymani` | Placeholder |
| `/about/riddhi` | Riddhi's page | `AboutController.Riddhi` | In progress |
| `/about/evan` | Evan's page | `AboutController.Evan` | Placeholder |
| `/destinations` | Destination card grid, filterable by `?continent=` | `DestinationsController.Index` | Done |
| `/destinations/table` | All database rows in a sortable HTML table (`?sort=`) | `DestinationsController.Table` | Done |
| `/destinations/{slug}` | Destination detail (404 for unknown slugs) | `DestinationsController.Details` | Done |
| `/gallery` | Gallery | `HomeController.Gallery` | Page shell and photo-grid partial ready; content in progress (Evan, #6) |
| `/tips` | Travel Tips | `HomeController.Tips` | Page shell and tip partial ready; content in progress (Evan, #6) |

## Running locally

Prerequisite: the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

**Rider:** open `Waypoint.slnx`, pick the `https` run configuration, and press Run.

**Command line:**

```bash
dotnet run --project Waypoint --launch-profile https
```

Then open <https://localhost:7085>.

### Database

Destinations are stored in SQL Server LocalDB (installed with Visual Studio or the SQL Server Express LocalDB package). The connection string `WaypointDb` is in `Waypoint/appsettings.json`, and `WaypointDbContext` is registered in `Program.cs` and injected into `DestinationsController`.

In Development the app applies pending migrations on startup, so the `Waypoint` database and its seed rows are created the first time you run it. To manage the schema by hand:

```bash
dotnet tool install --global dotnet-ef   # once
dotnet ef database update --project Waypoint
dotnet ef migrations add <Name> --project Waypoint
```

### Running on macOS

LocalDB is Windows-only, so on a Mac run SQL Server in Docker and point the app at it with [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets). The LocalDB string in `appsettings.json` stays as is for Windows, and user secrets override it only on your machine.

1. Install Docker Desktop. On Apple Silicon, turn on **Settings > General > Use Rosetta for x86_64/amd64 emulation** (the SQL Server image is x86-only).
2. Start a container. Pick your own strong SA password:
   ```bash
   docker run -d --platform linux/amd64 --name waypoint-sql \
     -e ACCEPT_EULA=Y -e "MSSQL_SA_PASSWORD=<your-password>" \
     -p 14333:1433 mcr.microsoft.com/mssql/server:2022-latest
   ```
3. Store the connection string as a user secret (kept in your user profile, never in the repo):
   ```bash
   dotnet user-secrets set "ConnectionStrings:WaypointDb" "Server=localhost,14333;Database=Waypoint;User Id=sa;Password=<your-password>;TrustServerCertificate=True;MultipleActiveResultSets=true" --project Waypoint
   ```
4. Run the app as usual. Migrations apply on startup. Later, `docker start waypoint-sql` brings the database back.

Seed rows live in `Waypoint/Data/SeedData.cs`. After changing them, add a new migration. Run the tests with:

```bash
dotnet test
```

## Gallery and Tips partials

Both pages already have their route, header, and layout. Content goes in through two shared partials so it matches the rest of the site.

**Gallery** (`Views/Home/Gallery.cshtml`): put images in `wwwroot/images/gallery/` (WebP, about 1600 px wide, under 300 KB), build a list of `GalleryPhoto`, and render the grid:

```cshtml
@{
    var photos = new List<GalleryPhoto>
    {
        new("/images/gallery/hilton-head-pier.webp", "Wooden pier over calm water at sunrise", "Hilton Head Island, 2024"),
        new("/images/gallery/kyoto-lanterns.webp", "Paper lanterns over a narrow street", "Kyoto", "Photographer Name", "https://unsplash.com/photos/..."),
    };
}
<partial name="_PhotoGrid" model="photos" />
```

`GalleryPhoto(Src, Alt, Caption, Credit?, CreditUrl?)`: alt text describes the scene for screen readers; leave the credit empty for your own photos, and fill both credit fields for stock photos (and add them to `wwwroot/images/CREDITS.md`).

**Tips** (`Views/Home/Tips.cshtml`): one heading per topic, one `_TipCard` per tip:

```cshtml
<h2 class="wp-title">Packing</h2>
<partial name="_TipCard" model='new Tip("Roll, don't fold", "Rolled clothes take less space and wrinkle less.")' />
```

## Design system

Tokens live at the top of `wwwroot/css/site.css`: sand (background), ink (text), ocean (links, focus, active states), and sunset (fills only: the primary button and the stop markers). Sunset is never used as text because it fails contrast on sand (1.87:1). Display type is Gloock, body is Public Sans, both self-hosted in `wwwroot/fonts`. Sections run full width with `--wp-gutter` padding; text width is limited inside grids, not by a centered page column.

## Branching workflow

```
feature/*  ──PR──▶  develop  ──PR──▶  main
```

- `main` is what gets graded and (later) deployed. Nobody pushes to it directly.
- `develop` is the integration branch. All work starts from it.
- Create a branch per task (`feature/about-riddhi`, `fix/navbar-dropdown`, `docs/readme`), push it, and open a pull request into `develop`.
- The `build` CI check must pass before merging. Merge with **Create a merge commit** so individual commits stay in history.
- Each team member edits only their own page (`Views/About/<Name>.cshtml`) to avoid merge conflicts.
- Add tests for new controller actions in `Waypoint.Tests` in the same PR.
- Commit messages follow [Conventional Commits](https://www.conventionalcommits.org/): `feat: add tips page`, `fix: correct slug route`, `docs: update readme`.

## Roadmap

**Phase 1 (check-in 1):** project setup, layout, Home and About controllers, CI. Then Destinations, Gallery, and Tips from each owner's feature branch.

**Phase 2: database (done)**
- EF Core with SQL Server LocalDB for development
- `Destination` model with a unique `Slug`
- `DbContext` registered in `Program.cs` and injected into controllers
- Connection string in `appsettings.json`, schema managed with migrations

**Phase 3: deployment (done)**: see [Deployment](#deployment).

## Deployment

```
GitHub Actions (main) --OIDC--> Azure App Service F1 (Windows, .NET 10)
                                   |  system-assigned managed identity
                                   v
                                Azure SQL Database (free offer, serverless)
```

- **Hosting:** Azure App Service Free (F1) on Windows, which now runs 64-bit apps on the free tier. HTTPS only, FTP and basic-auth publishing turned off.
- **Database:** Azure SQL Database free offer, set to auto-pause until next month when the free limit is reached, so it can never bill. The server uses Microsoft Entra-only authentication: there is no SQL password anywhere.
- **App to database:** the web app's system-assigned managed identity signs in to SQL (`Authentication=Active Directory Default`). Its database user has only `db_datareader` and `db_datawriter`. It cannot change the schema, so a bug or a compromised app cannot drop tables.
- **Deploys:** every push to `main` runs the `deploy` job after `build` passes. It signs in through OpenID Connect with a user-assigned managed identity whose federated credential trusts only this repo's `production` environment, and whose role is Website Contributor on this one web app. No credentials are stored in GitHub; `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, and `AZURE_SUBSCRIPTION_ID` are identifiers saved as environment variables. A smoke test then requests `/`, `/destinations`, and a detail page and fails the job on anything but 200.
- **Connection string:** set in App Service configuration, never in `appsettings*.json`. EF Core retries transient failures because the paused database takes a moment to resume.
- **Schema changes:** migrations do not run on startup in production. Run them from a signed-in machine with `infra/azure-migrate.sh`, which opens a firewall rule for your IP, applies migrations as the Entra admin, ensures the app's database user, and removes the rule.
- **Setup from scratch:** `infra/azure-setup.sh` creates every resource with the Azure CLI. A $5 monthly budget alert is the safety net.
