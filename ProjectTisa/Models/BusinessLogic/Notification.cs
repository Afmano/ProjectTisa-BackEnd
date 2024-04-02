using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectTisa.Models.Enums;
using System.Text.Json.Serialization;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model of notification for <see cref="NotificationsUser"/>.
    /// </summary>
    public class Notification
    {
        public Notification() { }
        [SetsRequiredMembers]
        public Notification(NotificationCreationReq request, EditInfo editInfo, int id = 0)
        {
            Id = id;
            Caption = request.Caption;
            Message = request.Message;
            NotificationType = request.NotificationType;
            EditInfo = editInfo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Caption { get; set; }
        public required string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public required virtual EditInfo EditInfo { get; set; }
        [JsonIgnore]
        public virtual List<User> Users { get; init; } = [];
    }
}
