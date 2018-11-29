using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Models
{
    internal class ExpiryDateAttribute : Attribute
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class ExpiryDateAttributeAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var isDateTime = DateTime.TryParse(Convert.ToString(value),
                    out DateTime dateTime);

                if (dateTime.Month >= DateTime.Now.Month)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage = "Invalid expiration date!");
                }
            }
        }
    }
}