using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for order by <see cref="Models.User"/>.
    /// </summary>
    public class Order
    {
        public Order() { }
        [SetsRequiredMembers]
        public Order(OrderCreationReq request, EditInfo editInfo, User user, List<ProductQuantity> productQuantities, decimal totalPrice = 0, int id = 0)
        {
            Id = id;
            User = user;
            UpdateNote = request.UpdateNote;
            Status = OrderStatus.InProgress;
            TotalPrice = totalPrice == 0 ? productQuantities.Select(x => x.Product.Price * x.Quantity).Sum() : totalPrice;
            ProductQuantities = productQuantities;
            EditInfo = editInfo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public virtual required User User { get; set; }
        public string? UpdateNote { get; set; }
        public required OrderStatus Status { get; set; }
        public required decimal TotalPrice { get; set; }
        public required virtual EditInfo EditInfo { get; set; }
        public virtual List<ProductQuantity> ProductQuantities { get; set; } = [];
    }
}
