using System.Globalization;
using System.Windows.Controls;

namespace IdeaSoft.Test.Desktop.UI.Validators
{
    public class StringEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, $"Campo obrigatório");
            }

            string stringValue = (string)value;
            RemoveBlankSpaces(ref stringValue);
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, $"Campo obrigatório");
            }

            return ValidationResult.ValidResult;
        }

        private void RemoveBlankSpaces(ref string value)
        {
            value = value.TrimEnd().TrimStart();
        }
    }
}
