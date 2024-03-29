using System.ComponentModel.DataAnnotations;
using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Validation.Enums;

namespace ProjectTisa.Controllers.GeneralData.Validation.Attributes
{
    public class StringRequirementsAttribute(StringMaxLengthType stringType = StringMaxLengthType.None, string regularExpression = ValidationConst.REGEX_NUM_SYMBS, int minStrLength = ValidationConst.MIN_STR_LENGTH) : ValidationAttribute
    {
        private readonly List<ValidationAttribute?> _attributes = [
            new RequiredAttribute(),
            new StringLengthAttribute((int)stringType),
            new MinLengthAttribute(minStrLength),
            regularExpression != null ? new RegularExpressionAttribute(regularExpression) : null
            ];
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<ValidationResult?> results = [];
            foreach (var attribute in _attributes)
            {
                results.Add(attribute?.GetValidationResult(value, validationContext));
            }
            return results.FirstOrDefault(res => res != null);
        }
    }
}
