using System.Security.Cryptography;

namespace ProjectTisa.Controllers.GeneralData
{
    /// <summary>
    /// Sub-record from appsettings.json for auth proccess.
    /// </summary>
    public record AuthData
    {
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required string IssuerSigningKey { get; init; }
        public required TimeSpan ExpirationTime { get; set; }
        public required int IterationCount { get; set; }
        public required int SaltSize { get; set; }
        public required string HashAlgorithmOID { get; set; }
        public HashAlgorithmName HashAlgorithm { get { return HashAlgorithmName.FromOid(HashAlgorithmOID); } }
    };
}
