using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Resources;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjectTisa.Controllers.GeneralData.Validation.Attributes
{
    /// <summary>
    /// Custom validation attribute to validate list of strings;
    /// </summary>
    /// <param name="regEx">Regular expression to validate in each string.</param>
    public class ListStringAttribute(string regEx = ValidationConst.REGEX_NUM_SYMBS, int minLength = ValidationConst.MIN_STR_LENGTH, int maxLength = ValidationConst.MAX_STR_LENGTH) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is List<string> list)
            {
                return list.All(item => Regex.Match(item, regEx).Success && item.Length >= minLength && item.Length <= maxLength) ? ValidationResult.Success : new($"{ResAnswers.ValidationErrorRegEx} {validationContext.DisplayName}" );
            }

            return ValidationResult.Success;
        }
    }
}
