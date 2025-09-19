namespace MoviesApi.Tests.Utilities
{
    /// <summary>
    /// Test project constants. Kept separate so tests can reference values without depending on the main project internals.
    /// </summary>
    internal static class Constants
    {
        internal const string InMemoryDbNameKey = "InMemoryDbName";

        internal const string EnableHttpsRedirectKey = "EnableHttpsRedirect";

        internal const int ReleaseYearMin = 1888;

        internal const int ReleaseYearMax = 9999;

        internal const int TestMovieReleaseYear = 2000;

        internal const string Slash = "/";

        internal const string N = "N";
        internal const string SlashMovies = "/Movies";

        internal const string TestMovieTitle = "Test Movie Title";

        internal const string TestMovieDirector = "Test Movie Director";

        internal const string TestMovieGenre = "Test Movie Genre";

        internal const string TestDbPrefix = "Test_MoviesDb_";

        internal const string False = "false";

        internal const string EnvironmentKey = "environment";

        internal const string Development = "Development";

        internal const string True = "true";
    }
}
