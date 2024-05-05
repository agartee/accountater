using System.ComponentModel.DataAnnotations;

namespace Accountater.WebApp.Attributes
{
    public class ValidCsvFileAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".csv" };
        private readonly int _maxFileSizeInMB;

        public ValidCsvFileAttribute(int maxFileSizeInMB = 2)
        {
            _maxFileSizeInMB = maxFileSizeInMB;
            ErrorMessage = "Invalid file";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
                return new ValidationResult("Please upload a file.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return new ValidationResult("Only CSV files are allowed.");

            if (file.Length > _maxFileSizeInMB * 1024 * 1024)
                return new ValidationResult($"File size cannot exceed {_maxFileSizeInMB} MB.");

            return ValidationResult.Success;
        }
    }
}
