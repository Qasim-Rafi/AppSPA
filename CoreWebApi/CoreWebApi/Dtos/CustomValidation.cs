using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class CustomValidation
    {
       
    }
    public class DateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex regex = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");

            //Verify whether date entered in dd/MM/yyyy format.
            bool isValid = regex.IsMatch(value.ToString().Trim());
            if (!isValid)
            {
                DateTime dt;
                isValid = DateTime.TryParseExact(value.ToString(), "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
                if (!isValid)
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
