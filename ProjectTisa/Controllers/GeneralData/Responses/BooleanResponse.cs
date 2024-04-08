namespace ProjectTisa.Controllers.GeneralData.Responses
{
    public class BooleanResponse(bool result)
    {
        public bool Result { get; set; } = result;
    }
}
