using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Enums
{
    public enum PaymentMethod
    {
        Check = 1,
        [Display(Name = "Direct Deposit")]
        DirectDeposit = 2,
        Cash = 3
    }
}
