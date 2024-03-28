using ProjectTisa.Controllers.GeneralData.Validation;
using ProjectTisa.Controllers.GeneralData.Validation.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="ProductController"/>.
    /// </summary>
    public record ProductCreationReq
    {
        public int Id { get; private init; }
        [StringRequirements]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        [Url]
        [StringRequirements(StringMaxLengthType.Domain)]
        public required string PhotoPath { get; set; }
        public required bool IsAvailable { get; set; }
        public int DiscountId { get; set; }
        public List<string> Tags { get; set; } = [];
        public int CategoryId { get; set; }
    }
}
