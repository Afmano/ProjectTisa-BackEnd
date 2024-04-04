namespace ProjectTisa.Controllers.GeneralData.Exceptions
{
    /// <summary>
    /// Custom extension to send via controller and handle it by application.
    /// </summary>
    public class ControllerException(string message) : Exception(message);
}
