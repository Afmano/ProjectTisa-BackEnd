namespace ProjectTisa.Controllers.GeneralData.Configs
{
    /// <summary>
    /// Config for access external storage.
    /// </summary>
    public record ExternalStorage
    {
        public required string PostUrl { get; init; }
        public required string GetUrl { get; init; }
        public required string Auth { get; init; }
    }
}
