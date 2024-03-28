using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for discounts of <see cref="Product"/>.
    /// </summary>
    public class Discount
    {
        public Discount() { }
        [SetsRequiredMembers]
        public Discount(DiscountCreationReq request, EditInfo editInfo, List<Product> products)
        {
            Name = request.Name;
            Description = request.Description;
            DiscountPercent = request.DiscountPercent;
            EditInfo = editInfo;
            Products = products;    
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal DiscountPercent { get; set; }
        public virtual required EditInfo EditInfo { get; set; }
        [JsonIgnore]
        public virtual List<Product> Products { get; set; } = [];
    }
}
