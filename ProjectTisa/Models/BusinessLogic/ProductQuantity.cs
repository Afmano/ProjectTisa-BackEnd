using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTisa.Models.BusinessLogic
{
    public class ProductQuantity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required int ProductId { get; set; }
        public required uint Quantity { get; set; }
    }
}
