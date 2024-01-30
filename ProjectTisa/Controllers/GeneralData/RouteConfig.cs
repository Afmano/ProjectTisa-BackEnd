namespace ProjectTisa.Controllers.GeneralData
{
    /// <summary>
    /// General config record for appsettings.json data.
    /// </summary>
    public record RouteConfig
    {
        public required string ApplicationName { get; init; }
        public required string Version { get; init; }
        public required string CurrentHost { get; init; }
        public required AuthData AuthData { get; init; }
        public required SmtpData SmtpData { get; init; }
    }
}
