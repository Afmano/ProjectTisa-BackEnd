using ProjectTisa.Controllers.GeneralData.Validation.Attributes;
using ProjectTisa.Controllers.GeneralData.Validation.Enums;
using System.ComponentModel.DataAnnotations;
using ProjectTisa.Controllers.BusinessControllers;
using ProjectTisa.Controllers.GeneralData.Consts;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="ProductController"/>.
    /// </summary>
    public record ProductCreationReq
    {
        [StringRequirements]
        public required string Name { get; set; }
        [Url]
        [StringRequirements(StringMaxLengthType.Domain, ValidationConst.NO_REGEX)]
        public required string PhotoPath { get; set; }
        [Range(0d, double.MaxValue)]
        public required decimal Price { get; set; }
        public required bool IsAvailable { get; set; }
        public string? Description { get; set; }
        [ListString]
        public List<string> Tags { get; set; } = [];
        public required int CategoryId { get; set; }
        public int DiscountId { get; set; }
    }
}
