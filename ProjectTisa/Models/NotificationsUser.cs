using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Models
{
    /// <summary>
    /// List of notification for <see cref="User"/>.
    /// </summary>
    public class NotificationsUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required int UserId { get; set; }
        public required List<Notification> Notifications { get; set; }
    }
}
