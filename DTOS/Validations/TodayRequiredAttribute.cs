using System.ComponentModel.DataAnnotations;

namespace DTOS.Validations
{
    public class TodayRequiredAttribute : ValidationAttribute
    {
        //Check if Datetime value is bigger or equals Today then the function is valid  
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success; //skip null check

            if (!(value is DateTime?))
            {
                // Value is not a DateTime or is null
                return new ValidationResult("Invalid date value.");
            }

            DateTime dateValue = (DateTime)value;

            if (dateValue.Date >= DateTime.Today)
            {
                // Date is greater than today
                return ValidationResult.Success;
            }

            // Date is not greater than today
            return new ValidationResult("Date must be greater than today.");
        }
    }
}