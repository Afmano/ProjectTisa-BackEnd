namespace ProjectTisa.Controllers.GeneralData.Responses
{
    /// <summary>
    /// Response class to return <see cref="string"/> JWT token and it exparation time as <see cref="DateTime"/>.
    /// </summary>
    public class TokenResponse(string token, DateTime exparationDateTime)
    {
        public string Token { get; set; } = token;
        public DateTime ExparationDate { get; set; } = exparationDateTime;
    }
}
