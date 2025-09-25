/*
  copilot-instructions.md for the MoviesApi project
  Purpose: give AI coding agents concise, actionable context to be immediately productive.
*/

# Project snapshot

- This repo contains a small ASP.NET Core Web API project at `MoviesApi/` (Target: `net9.0`).
- Key files:
  - `MoviesApi/Program.cs` — app startup and DI registration.
  - `MoviesApi/Models/Movie.cs` and `MoviesApi/Models/MovieContext.cs` — EF model & DbContext.
  - `MoviesApi/MoviesApi.csproj` — package references (EF Core, InMemory, NSwag/OpenAPI).

# Big picture / architecture

- Single ASP.NET Core Web API service serving controllers under `Controllers/`.
- Uses Entity Framework Core with an in-memory database by default (configured in `Program.cs`):
  `builder.Services.AddDbContext<MovieContext>(opt => opt.UseInMemoryDatabase("MoviesList"));`
- OpenAPI/Swagger is enabled in development via `Microsoft.AspNetCore.OpenApi` and `NSwag.AspNetCore`.
- No separate front-end or microservices in this repo — focus is API and examples.

# Important, discoverable project-specific notes (must-read for agents)

- MovieContext bug: `MovieContext` declares `public DbSet<MovieContext> MovieItems { get; set; }` but should expose the entity `Movie`:
  - Correct shape: `public DbSet<Movie> MovieItems { get; set; }`.
  - When making data-layer changes, check `MoviesApi/Models/MovieContext.cs` and update migrations/tests accordingly.
- Tests: there are no unit tests present. Any code changes that add behavior should include small unit/integration tests (xUnit or MSTest) and an example using the in-memory provider.

# Build / run / debug (developer workflows)

- Build and run the API locally (from repository root):

  - Build:

    dotnet build MoviesApi/MoviesApi.csproj

  - Run (development):

    dotnet run --project MoviesApi/MoviesApi.csproj --launch-profile "Development"

  - Or run without profile (uses Kestrel defaults):

    dotnet run --project MoviesApi/MoviesApi.csproj

- The project targets `.NET 9.0`. Use the corresponding SDK. If the host environment uses multiple SDKs, prefer the SDK that matches `TargetFramework` in `MoviesApi.csproj`.
- Debugging in VS Code: open the `MoviesApi` folder as a workspace folder, use the recommended `.NET Core Launch` configuration or run the project with `dotnet run` and attach the debugger.

# Conventions & patterns (specific to this codebase)

- Minimal API bootstrap pattern using `WebApplication.CreateBuilder(args)` and minimal Program.cs (top-level statements).
- Controllers follow default routing (e.g., `[Route("[controller]")]`). Use `MapControllers()` in `Program.cs`.
- OpenAPI docs are exposed under `/openapi/v1.json` and Swagger UI is enabled only in development via `app.MapOpenApi()` and `app.UseSwaggerUi(...)`.
- Entity classes initialize string properties to `string.Empty` in constructors (see `Movie`), prefer non-nullable strings with explicit initialization.

# Integration points & external dependencies

- NuGet packages (in `MoviesApi.csproj`):
  - `Microsoft.AspNetCore.OpenApi` (OpenAPI wiring)
  - `NSwag.AspNetCore` (Swagger UI)
  - `Microsoft.EntityFrameworkCore.InMemory` (default dev/test DB provider)
  - `Microsoft.EntityFrameworkCore.SqlServer` present but unused by default — remove or use when adding SQL Server support.

- No external services, secrets, or cloud wiring are present in the repo. If you add secrets/config, follow `appsettings.json` and `appsettings.Development.json` conventions already present in `MoviesApi/bin/Debug/net9.0/`.

# Coding agent guidance (how to author code changes here)

- Make minimal, focused changes. Keep `Program.cs` minimal — add services via `builder.Services` and keep request pipeline changes inside the environment checks.
- When modifying EF models:
  - Fix `MovieContext` DbSet type first (see bug above).
  - Prefer the InMemory provider for unit tests; for integration tests that simulate production, wire `SqlServer` and set configuration in test setup.
- When adding new controllers, follow existing pattern: create in `Controllers/`, use `[ApiController]`, and rely on DI for `ILogger<T>` or `MovieContext`.
- Keep OpenAPI visibility conditional on `app.Environment.IsDevelopment()` as in `Program.cs`.

# Examples (use these snippets when adding or fixing code)

- Fix DbSet example (edit `MoviesApi/Models/MovieContext.cs`):

  public DbSet<Movie> MovieItems { get; set; } = null!;

- Example controller injection (new controller):

  public class MoviesController : ControllerBase
  {
      private readonly MovieContext _db;
      public MoviesController(MovieContext db) => _db = db;
      // ... actions using _db.MovieItems
  }

# What not to change without verification

- Do not remove OpenAPI wiring without verifying calling clients — the repo expects OpenAPI only in development.
- Do not switch the DB provider in `Program.cs` from InMemory to SqlServer without adding configuration and migration steps.

# Where to look next (quick navigation)

- `MoviesApi/Program.cs` — app startup and OpenAPI wiring
- `MoviesApi/Models/MovieContext.cs` — DbContext (has a discovered bug)
- `MoviesApi/Models/Movie.cs` — entity shape and constructor nullability pattern
- `MoviesApi/Controllers/` — controllers (e.g., `MoviesController`)

# If you need human help

- Ask the maintainer to confirm whether `SqlServer` is a required target for CI or production; otherwise prefer `InMemory` for tests.

---
Please review these instructions and tell me if you'd like additional examples (tests, CI steps, or migration guidance) or if you want me to also fix the `MovieContext` DbSet type automatically.
