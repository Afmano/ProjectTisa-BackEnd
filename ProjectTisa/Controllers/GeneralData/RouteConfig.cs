namespace ProjectTisa.Controllers.GeneralData
{
    public record RouteConfig
    {
        public required string ApplicationName { get; set; }
        public required string Version { get; set; }
        public required string CurrentHost { get; set; }
        public required AuthData AuthData { get; set; }
    }
}
