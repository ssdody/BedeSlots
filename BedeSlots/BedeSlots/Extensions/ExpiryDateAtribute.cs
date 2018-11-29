using System;
using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpiryDateAttributeAttribute : ValidationAttribute
    {
        public ExpiryDateAttributeAttribute(string expiryDate)
        {
            this.ExpiryDate = expiryDate;
        }

        private string ExpiryDate { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime laterDate = (DateTime)validationContext.ObjectType.GetProperty(ExpiryDate).GetValue(validationContext.ObjectInstance, null);

            if (laterDate.Month >= DateTime.Now.Month)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid expiration date!");
            }
        }
    }

}
