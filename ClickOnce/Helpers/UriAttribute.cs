using System;
using System.ComponentModel.DataAnnotations;

namespace ClickOnce
{
    public class UriAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return null;
            }

            try
            {
                var _ = new Uri(value.ToString()); // This appears to be how ClickOnce handles Url validation...
                return null;
            }
            catch (Exception exception)
            {
                return new ValidationResult(exception.Message, new[] {validationContext.MemberName});
            }
        }
    }
}
