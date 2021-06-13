using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Validators {

    public class Availability : ValidationAttribute {
        
        public override string FormatErrorMessage(string name) {
            return $"{name} value should be a list of integers 1-7";
        }

        protected override ValidationResult IsValid(object value, ValidationContext context) {
            if(value is List<int> listValue) {
                foreach(int item in listValue) {
                    if(item < 1 || item > 7) {
                        return new ValidationResult(FormatErrorMessage(context.DisplayName));
                    }
                }
            }

            return ValidationResult.Success;
        }

    }

}
