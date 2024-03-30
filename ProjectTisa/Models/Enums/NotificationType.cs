using System.Text.Json.Serialization;

namespace ProjectTisa.Models.Enums
{
    /// <summary>
    /// Enum-type of <see cref="Notification"/>.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum NotificationType
    {
        None,
        Information,
        Warning,
        Error
    }
}
