using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for order by <see cref="User"/>.
    /// </summary>
    public class Order
    {
        public Order() { }
        [SetsRequiredMembers]
        public Order(OrderCreationReq request, EditInfo editInfo, User user, List<ProductQuantity> productQuantities)
        {
            User = user;
            UpdateNote = request.UpdateNote;
            TotalPrice = productQuantities.Select(x => x.Product.Price * x.Quantity).Sum();
            ProductQuantities = productQuantities;
            EditInfo = editInfo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public virtual required User User { get; set; }
        public string? UpdateNote { get; set; }
        public required decimal TotalPrice { get; set; }
        public required virtual EditInfo EditInfo { get; set; }
        public virtual List<ProductQuantity> ProductQuantities { get; set; } = [];
    }
}
