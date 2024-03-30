using ProjectTisa.Controllers.BusinessControllers;

namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="OrderController"/>.
    /// </summary>
    public record OrderCreationReq
    {
        public required int UserId { get; set; }
        public required List<ProductIdQuantity> ProductIdQuantities { get; set; } = [];
        public string? UpdateNote { get; set; }
        public record ProductIdQuantity
        {
            public int ProductId { get; set; }
            public uint Quantity { get; set; }
        }
    }
    
}
