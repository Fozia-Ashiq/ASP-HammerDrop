using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _conditionPropertyName;
        private readonly object _expectedValue;
        public RequiredIfAttribute(string conditionPropertyName, object expectedValue)
        {
            _conditionPropertyName = conditionPropertyName;
            _expectedValue = expectedValue;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conditionProperty = validationContext.ObjectType.GetProperty(_conditionPropertyName);
            if (conditionProperty == null)
            {
                return new ValidationResult($"Unknown property: {_conditionPropertyName}");
            }
            var conditionValue = conditionProperty.GetValue(validationContext.ObjectInstance);
            if (conditionValue != null && conditionValue.Equals(_expectedValue) && value == null)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required when {_conditionPropertyName} is {_expectedValue}");
            }
            return ValidationResult.Success;
        }
    }
}