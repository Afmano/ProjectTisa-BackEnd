using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for order by <see cref="User"/>.
    /// </summary>
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public virtual required User User { get; set; }
        public string? UpdateNote { get; set; }
        public required decimal TotalPrice { get; set; }
        public required virtual EditInfo EditInfo { get; set; }
        public virtual List<ProductQuantity> Products { get; set; } = [];
    }
}
