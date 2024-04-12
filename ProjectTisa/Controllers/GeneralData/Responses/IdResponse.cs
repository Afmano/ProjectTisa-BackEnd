namespace ProjectTisa.Controllers.GeneralData.Responses
{
    /// <summary>
    /// Response class to return <see cref="int"/> id of some entity.
    /// </summary>
    public class IdResponse(int id)
    {
        public int Id { get; set; } = id;
    }
}
