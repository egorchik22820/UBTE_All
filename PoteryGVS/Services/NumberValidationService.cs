using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PoteryGVS.Services
{
    public class NumberValidationService : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return new ValidationResult(false, "Значение не может быть пустым");

            if (int.TryParse(value.ToString(), out int number))
            {
                if (number >= 0 && number < 100)
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, "Число должно быть в диапазоне от 0 до 99");
            }

            return new ValidationResult(false, "Введите целое число");
        }
    }
}
