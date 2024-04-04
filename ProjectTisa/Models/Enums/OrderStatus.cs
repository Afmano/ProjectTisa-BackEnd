using System.Text.Json.Serialization;

namespace ProjectTisa.Models.Enums
{
    /// <summary>
    /// Define status of <see cref="BusinessLogic.Order"/>
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        InProgress,
        Completed,
        Cancelled
    }
}
