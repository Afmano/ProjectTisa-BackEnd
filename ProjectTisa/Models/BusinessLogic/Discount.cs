using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectTisa.Models.BusinessLogic
{
    public class Discount
    {
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
