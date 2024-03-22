using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectTisa.Models.Enums;

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
        public required DateTime CreationTime { get; set; }
        public TimeSpan Exparation { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
