using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Models
{
    /// <summary>
    /// List of notification for <see cref="Models.User"/>.
    /// </summary>
    public class NotificationsUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required User User { get; set; }
        public required List<Notification> Notifications { get; set; }
    }
}
