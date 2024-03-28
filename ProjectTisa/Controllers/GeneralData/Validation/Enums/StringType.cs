using ProjectTisa.Controllers.GeneralData.Consts;

namespace ProjectTisa.Controllers.GeneralData.Validation.Enums
{
    public enum StringMaxLengthType
    {
        None = ValidationConst.MAX_STR_LENGTH,
        Username = ValidationConst.MAX_USERNAME_LENGTH,
        Domain = ValidationConst.MAX_DOMAIN_LENGTH
    }
}
