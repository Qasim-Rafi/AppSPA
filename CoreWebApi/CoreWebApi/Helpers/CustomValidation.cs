using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class CustomValidation
    {

    }
    public class DateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex regex = new Regex(@"^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$");
            //return ValidationResult.Success;
            //Verify whether date entered in MM/dd/yyyy format.
            bool isValid = regex.IsMatch(value.ToString().Trim());
            if (isValid)
            {
                var dt = new DateTime();
                isValid = DateTime.TryParseExact(value.ToString(), "MM/dd/yyyy", null, DateTimeStyles.None, out dt);
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
