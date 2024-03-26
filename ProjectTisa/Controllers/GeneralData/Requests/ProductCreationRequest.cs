using ProjectTisa.Models.BusinessLogic;

namespace ProjectTisa.Controllers.GeneralData.Requests
{
    public class ProductCreationRequest 
    {
        public int Id { get; private init; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required string PhotoPath { get; set; }
        public required bool IsAvailable { get; set; }
        public int DiscountId { get; set; }
        public List<string> Tags { get; set; } = [];
        public int CategoryId { get; set; }
    }
}
