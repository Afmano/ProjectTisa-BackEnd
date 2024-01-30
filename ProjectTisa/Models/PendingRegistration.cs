using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Models
{
    /// <summary>
    /// Pending request to verify registration.
    /// </summary>
    public class PendingRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Username { get; init; }
        public required string Email { get; init; }
        public required string PasswordHash { get; init; }
        public required string Salt { get; init; }
        public DateTime ExpireDate { get; init; }
        public required string VerificationCode { get; init; } 
    }
}
