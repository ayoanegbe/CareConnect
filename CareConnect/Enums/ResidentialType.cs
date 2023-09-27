using System.ComponentModel.DataAnnotations;

namespace CareConnect.Enums
{
    public enum ResidentialType
    {
        [Display(Name = "Supportive Roomate")]
        SupportiveRoomate = 1,
        [Display(Name = "Group Home")]
        GroupHome = 2,
        [Display(Name = "Supported Independent Living")]
        SupportedIndependentLiving = 3
    }
}
