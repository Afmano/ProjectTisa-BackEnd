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
        public required TimeSpan ExpirationTime { get; init; }
        public required int IterationCount { get; init; }
        public required int SaltSize { get; init; }
        public required string HashAlgorithmOID { get; init; }
        public HashAlgorithmName HashAlgorithm { get { return HashAlgorithmName.FromOid(HashAlgorithmOID); } }
    };
}
