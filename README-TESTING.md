Debugging unit tests in this repository

This project includes a `MoviesApi` ASP.NET Core API and `MoviesApi.Tests` unit tests.

VS Code launch configurations

Open the Run and Debug sidebar (Cmd+Shift+D) and choose one of the configurations in `.vscode/launch.json`:

- "Debug All Tests (dotnet test)"
  - Runs `dotnet test` for `MoviesApi.Tests` and attaches the debugger.
  - This configuration now builds before running tests.

- "Debug Single Test (prompt for filter)"
  - Prompts you for a dotnet test `--filter` expression and runs only matching tests with the debugger attached.
  - Example filters:
    - `FullyQualifiedName~MoviesApi.Controllers.MoviesControllerTest.Create_AddsMovie_ReturnsCreated`
    - `DisplayName~Create_AddsMovie_ReturnsCreated`

Tips

- Set breakpoints in the test or source code before starting a debug session.
- If you change test code, the debugger will build the project before running (the launch configs are configured to build).
- You can also run tests manually from the terminal:

```bash
# Build
dotnet build MoviesApi.Tests/MoviesApi.Tests.csproj

# Run all tests
dotnet test MoviesApi.Tests/MoviesApi.Tests.csproj

# Run a single test using a filter
dotnet test MoviesApi.Tests/MoviesApi.Tests.csproj --filter "FullyQualifiedName~MoviesApi.Controllers.MoviesControllerTest.Create_AddsMovie_ReturnsCreated"
```

Troubleshooting

- If breakpoints don't hit, ensure the build configuration is `Debug` (default for dotnet test) and that PDBs were generated.
- Remove `--no-build` from test args (already removed) if you want the debug launch to compile before running.

If you want, I can also append the additional `MoviesController` unit tests we discussed to `MoviesApi.Tests/Controllers/MoviesControllerTests.cs` and run the tests again.
