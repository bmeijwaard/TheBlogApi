using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Helpers.Attributes
{
    public class SanatizeHtmlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = (string)validationContext.ObjectInstance;

            // TODO currently the output can only be validated, while it should be cleaned and returned.
            if (string.IsNullOrEmpty(text))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"This html text contains unsafe content.";
        }
    }
}
