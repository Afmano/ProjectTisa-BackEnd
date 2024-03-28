using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Libs;
using ProjectTisa.Controllers.GeneralData.Requests.UserReq;

namespace ProjectTisa.Models
{
    /// <summary>
    /// Pending request to verify registration.
    /// </summary>
    public class PendingRegistration
    {
        public PendingRegistration() { }
        [SetsRequiredMembers]
        public PendingRegistration(UserInfoReq userCreation, string passSalt, string passHash, string verificationCode)
        {
            Username = userCreation.Username.ToLower();
            Email = userCreation.Email.ToLower();
            ExpireDate = DateTime.UtcNow.AddHours(2);
            Salt = passSalt;
            PasswordHash = passHash;
            VerificationCode = verificationCode;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string PasswordHash { get; init; }
        public required string Salt { get; init; }
        public required DateTime ExpireDate { get; init; }
        public required string VerificationCode { get; init; } 
    }
}
