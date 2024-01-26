using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectTisa.Models
{
    /// <summary>
    /// Model for user.
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? PhotoPath { get; set; }
        [JsonIgnore]
        public string? PasswordHash { get; set; }
        [JsonIgnore]
        public string? Salt { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastSeen { get; set; }
        public override string ToString()
        {
            return $"{Id}. {Username}, {Email}, {PhotoPath}";
        }
    }
}
