using System.ComponentModel.DataAnnotations;

namespace BedeSlots.Data.Models
{
    public enum CardType
    {
        [Display(Name = "Visa")]
        Visa = 1,
        [Display(Name = "Master Card")]
        MasterCard = 2,
        [Display(Name = "American Express")]
        AmericanExpress = 3
    }
}
