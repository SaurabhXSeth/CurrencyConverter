using System.ComponentModel.DataAnnotations;
namespace Models
{
    public enum ValidationResult
    {
        [Display(Name = "Valid", Description = "Valid")]
        Valid = 0,

        [Display(Name = "InvalidSourceCurrency", Description = "Invalid Source Currency")]
        InvalidSourceCurrency = 1,

        [Display(Name = "InvalidTargetCurrency", Description = "Invalid Target Currency")]
        InvalidTargetCurrency = 2,

        [Display(Name = "PriceOverFlow", Description = "Price Over Flow")]
        PriceOverFlow = 3,

        [Display(Name = "InvalidPrice", Description = "Invalid Price")]
        InvalidPrice = 4,

        [Display(Name = "InValidInput", Description = "In Valid Input")]
        InValidInput = 5,
    }
}
