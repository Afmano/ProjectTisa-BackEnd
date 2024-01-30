namespace ProjectTisa.Controllers.GeneralData
{
    /// <summary>
    /// Sub-record from appsettings.json for SMTP email sending.
    /// </summary>
    public record class SmtpData
    {
        public required int Port { get; init; }
        public required string FromEmail { get; init; }
        public required string Password { get; init; }
        public required bool DefaultCredentais { get; init; } = false;
        public required bool Ssl { get; init; } = true;
    }
}
