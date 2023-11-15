using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Enums
{
    public enum ImmigrationStatus
    {
        Citizen = 1,
        [Display(Name = "Permanent Resident")]
        PermanentResident = 2,
        [Display(Name = "Work Permit")]
        WorkPermit = 3,
    }
}
