namespace ProjectTisa.Controllers.GeneralData.Responses
{
    /// <summary>
    /// Response class to return <see cref="string"/> message.
    /// </summary>
    public class MessageResponse(string messsage)
    {
        public string Message { get; set; } = messsage;
    }
}
