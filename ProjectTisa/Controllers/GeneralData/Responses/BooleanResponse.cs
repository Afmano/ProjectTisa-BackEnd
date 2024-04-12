namespace ProjectTisa.Controllers.GeneralData.Responses
{
    /// <summary>
    /// Response class to return <see cref="bool"/> value.
    /// </summary>
    public class BooleanResponse(bool result)
    {
        public bool Result { get; set; } = result;
    }
}
