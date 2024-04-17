namespace ProjectTisa.Controllers.GeneralData.Responses
{
    /// <summary>
    /// Response class to return <see cref="string"/> JWT token and it exparation time as <see cref="DateTime"/>.
    /// </summary>
    public record TokenResponse(string Token, DateTime ExparationDateTime);
}
