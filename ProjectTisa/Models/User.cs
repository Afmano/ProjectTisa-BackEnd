using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectTisa.Models.Const;
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
        [RegularExpression(ValidationConst.REGEX_NUM_SYMBS)]
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Username { get; set; }
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Email { get; set; }
        [StringLength(ValidationConst.MAX_PATH_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public string? PhotoPath { get; set; }
        [JsonIgnore]
        [MaxLength(ValidationConst.MAX_HASH_LENGTH)]
        public string? PasswordHash { get; set; }
        [JsonIgnore]
        [MaxLength(ValidationConst.MAX_HASH_LENGTH)]
        public string? Salt { get; set; }
        [JsonIgnore]
        public DateTime RegistrationDate { get; set; }
        [JsonIgnore]
        public DateTime? LastSeen { get; set; }
        public override string ToString()
        {
            return $"{Id}. {Username}, {Email}, {PhotoPath}";
        }
    }
}
