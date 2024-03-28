namespace ProjectTisa.Controllers.GeneralData.Requests.CreationReq
{
    /// <summary>
    /// DTO for <see cref="OrderController"/>.
    /// </summary>
    public record OrderCreationReq
    {
        public required int UserId { get; set; }
        public required List<int> ProductIds { get; set; } = [];
        public string? UpdateNote { get; set; }
    }
}
