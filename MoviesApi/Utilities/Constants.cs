namespace MoviesApi.Utilities;

/// <summary>
/// Application-wide constant values and configuration keys.
/// All members are public so they can be used across projects and tests.
/// </summary>
public static class Constants
{
    // Configuration keys
    public const string InMemoryDbNameKey = "InMemoryDbName";
    public const string EnableHttpsRedirectKey = "EnableHttpsRedirect";

    // Default values and bounds used by controllers/DTO validation
    public const int ReleaseYearMin = 1888; // earliest commonly-accepted film year
    public const int ReleaseYearMax = 9999; // allow distant future for testability

    // Test helpers / prefixes
    public const string TestDbPrefix = "Test_MoviesDb_";
}
