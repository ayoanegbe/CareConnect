using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Enums
{
    public enum RelationshipType
    {
        Father = 1,
        Mother = 2,
        Brother = 3,
        Sister = 4,
        Guardian = 5,
        Doctor = 6,
        Psychiatrist = 7,
        Trustee = 8,
        [Display(Name = "Room Mate")]
        RoomMate = 9,
    }
}
