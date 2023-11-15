using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Enums
{
    public enum ApplicationStatus
    {
        Fresh = 1,
        Interview = 2,
        [Display(Name = "Keep In View")]
        KeepInView = 3,
        Successfull = 4,
        [Display(Name = "Not Succcessful")]
        NotSucccessful = 5
    }
}
