﻿using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Resources;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjectTisa.Controllers.GeneralData.Validation.Attributes
{
    public class ListStringAttribute(string regEx = ValidationConst.REGEX_NUM_SYMBS) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is List<string> list)
            {
                return list.All(item => Regex.Match(item, regEx).Success) ? ValidationResult.Success : new(ResAnswers.ValidationErrorRegEx);
            }

            return ValidationResult.Success;
        }
    }
}
