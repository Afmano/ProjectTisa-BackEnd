using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Controllers.GeneralData.Validation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="DiscountController"/>.
    /// </summary>
    public record DiscountCreationReq
    {
        [StringRequirements]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Range(0d, 1d)]
        public required decimal DiscountPercent { get; set; }
        public List<int> ProductIds { get; set; } = [];

    }
}
