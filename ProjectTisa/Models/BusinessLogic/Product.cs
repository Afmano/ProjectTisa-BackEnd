using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for product. Main model for logic.
    /// </summary>
    public class Product
    {
        public Product() { }
        [SetsRequiredMembers]
        public Product(ProductCreationReq registration, EditInfo editInfo, Category category, Discount? discount)
        {
            Name = registration.Name;
            PhotoPath = registration.PhotoPath;
            Description = registration.Description;
            Price = registration.Price;
            IsAvailable = registration.IsAvailable;
            Tags = registration.Tags;
            Discount = discount;
            Category = category;
            EditInfo = editInfo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Name { get; set; }
        public required string PhotoPath { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required bool IsAvailable { get; set; }
        public List<string> Tags { get; set; } = [];
        public virtual Discount? Discount { get; set; }
        public required virtual Category Category { get; set; }
        public required virtual EditInfo EditInfo { get; set; }
    }
}
