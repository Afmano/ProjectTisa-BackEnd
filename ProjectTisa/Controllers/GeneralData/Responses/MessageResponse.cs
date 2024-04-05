namespace ProjectTisa.Controllers.GeneralData.Responses
{
    public class MessageResponse(string messsage)
    {
        public string Message { get; set; } = messsage;
    }
}
