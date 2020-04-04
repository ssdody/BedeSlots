using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Common.CustomAttributes
{
    public class ExpiryDateAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "The date should be in the future!";
        }

        protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        {
            var isDateTime = DateTime.TryParse(Convert.ToString(objValue), out DateTime dateValue);

            if (dateValue <= DateTime.Now)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}