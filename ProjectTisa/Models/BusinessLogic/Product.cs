using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTisa.Models.BusinessLogic
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required string PhotoPath { get; set; }
        public required bool IsAvailable { get; set; }
        public virtual Discount? Discount { get; set; }
        public List<string> Tags { get; set; } = [];
        public required virtual Category Category { get; set; }
        public required virtual EditInfo EditInfo { get; set; }
    }
}
