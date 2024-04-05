namespace ProjectTisa.Controllers.GeneralData.Responses
{
    public class TokenResponse(string token, DateTime exparationDateTime)
    {
        public string Token { get; set; } = token;
        public DateTime ExparationDate { get; set; } = exparationDateTime;
    }
}
