using ProjectTisa.Controllers.GeneralData.Validation.Attributes;
using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Models.Enums;
using ProjectTisa.Controllers.GeneralData.Consts;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="NotificationController"/>.
    /// </summary>
    public record NotificationCreationReq
    {
        [StringRequirements(Validation.Enums.StringMaxLengthType.None, ValidationConst.REGEX_NUM_SYMBS_SPACE)]
        public required string Caption { get; set; }
        public required string Message { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
