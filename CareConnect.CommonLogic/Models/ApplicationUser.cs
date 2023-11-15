using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.CommonLogic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        : base()
        {
        }

        public ApplicationUser(string userName, string firstName, string lastName, string phone)
        : base(userName)
        {
            base.Email = userName;
            base.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
        }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public bool ChangePassword { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; } = null;

        public string FullName => $"{this.FirstName} {this.LastName}";
        public string UserAlias => $"{this.FirstName}.{this.LastName}";
    }
}
