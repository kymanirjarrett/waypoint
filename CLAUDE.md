# Waypoint

Travel destination guide for IT3047C (University of Cincinnati). ASP.NET Core MVC on .NET 10, Razor views with Bootstrap 5 and jQuery, EF Core 10 on SQL Server, xUnit tests.

## Run and test

```bash
dotnet run --project Waypoint --launch-profile https   # https://localhost:7085
dotnet test
```

- Windows: the LocalDB string in `Waypoint/appsettings.json` works as is.
- macOS: LocalDB is Windows-only. Run SQL Server in Docker and override the string with user secrets. See "Running on macOS" in the README.
- Development applies pending migrations on startup. New schema changes need a new migration (`dotnet ef migrations add <Name> --project Waypoint`). Never edit an existing migration.

## Workflow

- `feature/*` (or `fix/`, `docs/`, `chore/`) branches off `develop`, PR into `develop`, then a release PR `develop` into `main`.
- Never push to `main` directly.
- The CI job `build` is the required check. It must pass before merge, and the job name must not change.
- Conventional Commits, one line (`feat(destinations): add photo credits`).
- Merge with "Create a merge commit". Do not rewrite pushed history.

## No AI attribution

Commits, PR titles and bodies, README, and code comments carry no AI attribution: no `Co-Authored-By` trailers, no "Generated with" lines, no session links. `.claude/settings.json` enforces this for Claude Code. Do not change it.

## Who owns what

- Each person edits only their own About page: `Views/About/Kymani.cshtml`, `Riddhi.cshtml`, `Evan.cshtml`.
- Gallery and Tips content (`Views/Home/Gallery.cshtml`, `Tips.cshtml`) is Evan's.
- Destinations data (`Data/SeedData.cs`) is Riddhi's. Ask before changing her rows.
- Restyling someone else's page (markup, classes, shared partials) is fine. Changing their words is not.

## Design system

- Tokens live at the top of `Waypoint/wwwroot/css/site.css`. Use them, not raw hex values.
- Sunset (`--wp-sunset`) is a fill only, never text: it is 1.87:1 on sand. Ink text on a sunset fill is 7.10:1.
- At most one accent element per screen. No accent-colored words in headlines.
- Layouts run full width with `--wp-gutter`. No content trapped in a centered fixed-width column; limit line length inside grids (`.wp-measure`).
- Stay on Bootstrap 5 + jQuery. No new frameworks or build step. Fonts are self-hosted in `wwwroot/fonts`.
- Check contrast with the landing-pages palette script before adding a color pairing.

## Rubric items that must not break

- LocalDB connection string `WaypointDb` in `Waypoint/appsettings.json`
- `WaypointDbContext` registered in `Program.cs` and injected into controllers
- The `Waypoint/Migrations` folder
- `Views/Shared/_Layout.cshtml`, `Views/_ViewStart.cshtml`, `Views/_ViewImports.cshtml`
- `wwwroot` with Bootstrap and jQuery under `wwwroot/lib`
- `/destinations/table` stays a real HTML `<table>`
