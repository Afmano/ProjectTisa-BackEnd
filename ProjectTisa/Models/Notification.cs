using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectTisa.Models.Enums;
using System.Text.Json.Serialization;

namespace ProjectTisa.Models
{
    /// <summary>
    /// Model of notification for <see cref="NotificationsUser"/>.
    /// </summary>
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Caption { get; set; }
        public required string Message { get; set; }
        public DateTime CreationTime { get; set; }
        public NotificationType NotificationType { get; set; }
        [JsonIgnore]
        public virtual List<User> Users { get; } = [];
    }
}
