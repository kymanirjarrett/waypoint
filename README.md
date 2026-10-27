# Waypoint

[![CI](https://github.com/kymanirjarrett/waypoint/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/kymanirjarrett/waypoint/actions/workflows/ci.yml)

Waypoint is a travel destination guide built with ASP.NET Core MVC on .NET 10. It's a team project for IT3047C Web Server Application Development at the University of Cincinnati, documenting our shared hobby: travel.

## Team

| Member | GitHub | Owns |
|---|---|---|
| Kymani Jarrett | [@kymanirjarrett](https://github.com/kymanirjarrett) | Project setup, layout and navigation, About pages, CI |
| Riddhi Mahajan | [@riddhuu](https://github.com/riddhuu) | Destinations (list, detail pages, slug routing) |
| Evan Schmidt | TBD | Gallery and Travel Tips |

Everyone owns their own About page (`Views/About/<Name>.cshtml`).

## Tech stack

- .NET 10, ASP.NET Core MVC (C#)
- Razor views with Bootstrap 5 and jQuery
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
| `/about/riddhi` | Riddhi's page | `AboutController.Riddhi` | Placeholder |
| `/about/evan` | Evan's page | `AboutController.Evan` | Placeholder |
| `/destinations` | Destination card grid | `DestinationsController.Index` | Planned (Riddhi) |
| `/destinations/{slug}` | Destination detail (404 for unknown slugs) | `DestinationsController.Details` | Planned (Riddhi) |
| `/gallery` | Gallery | `HomeController.Gallery` | Planned (Evan) |
| `/tips` | Travel Tips | `HomeController.Tips` | Planned (Evan) |

## Running locally

Prerequisite: the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

**Rider:** open `Waypoint.slnx`, pick the `https` run configuration, and press Run.

**Command line:**

```bash
dotnet run --project Waypoint --launch-profile https
```

Then open <https://localhost:7085>. Run the tests with:

```bash
dotnet test
```

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

**Phase 2: database**
- EF Core with SQL Server LocalDB for development
- `Destination` model with a unique `Slug`
- `DbContext` registered in `Program.cs` and injected into controllers
- Connection string in `appsettings.Development.json`, schema managed with migrations

**Phase 3: deployment**
- Deploy `main` to Azure App Service (F1 tier) via GitHub Actions
- Azure SQL free offer in production
- Connection string in App Service configuration, never committed
- Managed Identity for passwordless database access
