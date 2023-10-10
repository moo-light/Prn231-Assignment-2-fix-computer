using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DTOS.Validations
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateRangeAttribute(string minDate)
        {
            if (!DateTime.TryParseExact(minDate, "MM/dd/yyyy", null, DateTimeStyles.None, out _minDate))
            {
                throw new ArgumentException("Invalid minDate format. Use 'MM/dd/yyyy'.");
            }
            _maxDate = DateTime.MaxValue;
        }

        public DateRangeAttribute(string minDate, string maxDate)
        {
            if (!DateTime.TryParseExact(minDate, "MM/dd/yyyy", null, DateTimeStyles.None, out _minDate))
            {
                throw new ArgumentException("Invalid minDate format. Use 'MM/dd/yyyy'.");
            }

            if (!DateTime.TryParseExact(maxDate, "MM/dd/yyyy", null, DateTimeStyles.None, out _maxDate))
            {
                throw new ArgumentException("Invalid maxDate format. Use 'MM/dd/yyyy'.");
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            if (!(value is DateTime))
            {
                return new ValidationResult("Invalid date value.");
            }

            DateTime dateValue = (DateTime)value;

            if (dateValue >= _minDate && dateValue <= _maxDate)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Date must be between {_minDate:MM/dd/yyyy} and {_maxDate:MM/dd/yyyy}.");
        }
    }
}