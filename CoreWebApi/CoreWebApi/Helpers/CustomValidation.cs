using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class DateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Regex regex = new Regex(@"^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$");

            //Verify whether date entered in MM/dd/yyyy format.
            bool isValid = true; //regex.IsMatch(value.ToString().Trim());
            if (isValid)
            {
                isValid = DateTime.TryParseExact(value.ToString(), "MM/dd/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime dt);
                if (isValid)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(ErrorMessage);
            }
            else
                return new ValidationResult(ErrorMessage);
        }
    }
    public class BoolValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isValid = (Convert.ToBoolean(value) == true || Convert.ToBoolean(value) == false) ? true : false;
            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
