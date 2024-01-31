using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTisa.Models
{
    /// <summary>
    /// Model for user.
    /// </summary>
    public class User
    {
        public User() { }
        [SetsRequiredMembers]
        public User(PendingRegistration registration)
        {
            Username = registration.Username;
            Email = registration.Email;
            PasswordHash = registration.PasswordHash;
            Salt = registration.Salt;
            RegistrationDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Username { get; init; }
        public required string Email { get; init; }
        [JsonIgnore]
        public string? PasswordHash { get; set; }
        [JsonIgnore]
        public string? Salt { get; init; }
        public DateTime RegistrationDate { get; init; }
        public DateTime? LastSeen { get; set; }
        public override string ToString()
        {
            return $"{Id}. {Username} - {Email}";
        }
    }
}
