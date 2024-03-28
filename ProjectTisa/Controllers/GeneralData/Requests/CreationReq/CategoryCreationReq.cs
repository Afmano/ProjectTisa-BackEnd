using ProjectTisa.Controllers.BusinessControllers;
using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Validation;
using ProjectTisa.Controllers.GeneralData.Validation.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="CategoryController"/>.
    /// </summary>
    public record CategoryCreationReq
    {
        [StringRequirements]
        public required string Name { get; set; }
        [Url]
        [StringRequirements(StringMaxLengthType.Domain, ValidationConst.NO_REGEX)]
        public required string PhotoPath { get; set; }
        public int CategoryId { get; set; }
    }
}
