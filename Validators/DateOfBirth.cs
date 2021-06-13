using System;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Validators {

    public class DateOfBirth : ValidationAttribute {

        public override string FormatErrorMessage(string name) {
            return $"{name} value should not be a future date";
        }

        protected override ValidationResult IsValid(object value, ValidationContext context) {
            DateTime? dateValue = value as DateTime?;
            
            if(dateValue != null && dateValue.Value > DateTime.Now) {
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }

    }

}
