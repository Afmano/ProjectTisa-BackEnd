using ProjectTisa.Controllers.GeneralData.Validation.Attributes;
using ProjectTisa.Models.Enums;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    public class NotificationCreationReq
    {
        [StringRequirements(Validation.Enums.StringMaxLengthType.None)]
        public required string Caption { get; set; }
        public required string Message { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
