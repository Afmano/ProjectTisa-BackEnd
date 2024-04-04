using ProjectTisa.Controllers.GeneralData.Consts;

namespace ProjectTisa.Controllers.GeneralData.Validation.Enums
{
    /// <summary>
    /// String type for max length in <see cref="Attributes.StringRequirementsAttribute"/>.
    /// </summary>
    public enum StringMaxLengthType
    {
        None = ValidationConst.MAX_STR_LENGTH,
        Username = ValidationConst.MAX_USERNAME_LENGTH,
        Domain = ValidationConst.MAX_DOMAIN_LENGTH
    }
}
