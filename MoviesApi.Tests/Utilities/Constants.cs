namespace MoviesApi.Tests.Utilities;

/// <summary>
/// Test project constants. Kept separate so tests can reference values without depending on the main project internals.
/// </summary>
public static class Constants
{
    public const string InMemoryDbNameKey = "InMemoryDbName";
    public const string EnableHttpsRedirectKey = "EnableHttpsRedirect";

    public const int ReleaseYearMin = 1888;
    public const int ReleaseYearMax = 9999;

    public const string TestDbPrefix = "Test_MoviesDb_";
}
