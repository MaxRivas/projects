# MoviesApi — How to debug locally

Quick reference for starting and debugging the `MoviesApi` Web API in VS Code or terminal.

Launch from VS Code (recommended):

- Open the workspace in VS Code.
- Run & Debug > select `Launch MoviesApi (Kestrel)` to run the built `MoviesApi.dll` and attach the debugger.
- Or select `Launch MoviesApi (with launch-profile)` to run using `dotnet run --launch-profile "Development"` behavior.

Attach to a running process:

- Start the app manually:

  dotnet run --project MoviesApi/MoviesApi.csproj --launch-profile "Development"

- In VS Code choose `Attach to MoviesApi (process)` and pick the dotnet process.

Build from terminal:

  dotnet build MoviesApi/MoviesApi.csproj

Notes:
- The launch configs expect the project target `net9.0` and produce `MoviesApi/bin/Debug/net9.0/MoviesApi.dll` when building.
- The launch-profile config uses `${command:dotnetExecutable}` so the dotnet CLI in PATH is used.
