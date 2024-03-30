using System.Text.Json.Serialization;

namespace ProjectTisa.Models.Enums
{
    /// <summary>
    /// Enum-type role of <see cref="Models.User"/>.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        User,
        Manager,
        Admin,
        SuperAdmin,
    }
}
