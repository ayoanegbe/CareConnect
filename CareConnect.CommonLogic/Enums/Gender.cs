using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Enums
{
    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3,
        [Display(Name = "Prefer not to disclose")]
        NonDisclose = 4
    }
}
