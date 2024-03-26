namespace ProjectTisa.Controllers.GeneralData.Requests
{
    public class CategoryCreationRequest 
    {
        public required string Name { get; set; }
        public required string PhotoPath { get; set; }
        public int CategoryId { get; set; }
    }
}
