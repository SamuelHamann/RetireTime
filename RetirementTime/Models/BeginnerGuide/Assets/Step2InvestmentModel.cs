using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Assets;

/// <summary>
/// Model for Step 2 Investment Accounts page
/// </summary>
public class Step2InvestmentModel
{
    public List<InvestmentAccountData> Accounts { get; set; } = new();
}

public class InvestmentAccountData
{
    public int? Id { get; set; }
    
    [Required(ErrorMessage = "Please enter a name for this account")]
    [MinLength(1, ErrorMessage = "Account name cannot be empty")]
    public string AccountName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please select an account type")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid account type")]
    public int AccountTypeId { get; set; }
    
    public bool IsBulkAmount { get; set; } = true;
    
    [RequiredIf(nameof(IsBulkAmount), true, ErrorMessage = "Please enter the total value")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal? BulkAmount { get; set; }
    
    public List<StockData> Stocks { get; set; } = new();
}

public class StockData
{
    public int? Id { get; set; }
    
    [Required(ErrorMessage = "Please enter a ticker symbol")]
    [MinLength(1, ErrorMessage = "Ticker symbol cannot be empty")]
    public string TickerSymbol { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter an amount")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
}

/// <summary>
/// Custom validation attribute for conditional required fields
/// </summary>
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _propertyName;
    private readonly object _desiredValue;

    public RequiredIfAttribute(string propertyName, object desiredValue)
    {
        _propertyName = propertyName;
        _desiredValue = desiredValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_propertyName);
        if (property == null)
        {
            return new ValidationResult($"Unknown property: {_propertyName}");
        }

        var propertyValue = property.GetValue(validationContext.ObjectInstance);
        
        if (propertyValue?.Equals(_desiredValue) == true)
        {
            if (value == null || (value is decimal d && d <= 0))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required");
            }
        }

        return ValidationResult.Success;
    }
}


