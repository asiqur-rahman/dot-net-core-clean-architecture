using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Project.Core.Entities.Common
{
    [Table("Users")]
    public class User : IdentityUser<int>
    {
        [Display(Name = "Full Name"), Required]
        [StringLength(150, ErrorMessage = "{0} can have a max of {1} characters")]
        public string FullName { get; set; }
        [Display(Name = "E-Mail"), Required, EmailAddress]
        [StringLength(50, ErrorMessage = "{0} can have a max of {1} characters")]
        [RegularExpression(@"\b[A - Z0 - 9._ % +-] +@[A-Z0-9.-]+\.[A-Z]{2,4}\b")]
        public override string Email { get; set; }
        [Display(Name = "User Name"), Required]
        [StringLength(15, ErrorMessage = "{0} can have a max of {1} characters")]
        public override string UserName { get; set; }
        [Display(Name = "User Password"), Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string PreviousPassword { get; set; }
        [Display(Name = "Date Of Birth"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? Dob { get; set; }
        [Display(Name = "NID"), StringLength(20, ErrorMessage = "{0} can have a max of {1} characters")]
        public string Nid { get; set; }

        [Display(Name = "MAC Address"), StringLength(35, ErrorMessage = "{0} can have a max of {1} characters"), Required]
        public string MAC { get; set; } = "00:00:00:00:00:00";

        [Display(Name = "IP Address"), StringLength(35, ErrorMessage = "{0} can have a max of {1} characters"), Required]
        public string IP { get; set; } = "000.000.000.000";
        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }
        [Display(Name = "Signature")]
        public string SignatureImage { get; set; }
        public int? ParentAgentUserId { get; set; }
        public int RoleId { get; set; }
        public int BranchId { get; set; }
        [Display(Name = "Employee ID"), StringLength(15, ErrorMessage = "{0} can have a max of {1} characters")]
        public string EmployeeId { get; set; }
        [Display(Name = "Finger Print")]
        public string FingerPrint { get; set; }
        public int? EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Vacation { get; set; }
        public bool ActiveStatus { get; set; }
        public bool IsAllBranchPermitted { get; set; }
        public bool IsSelectedBranchAllAgentPermitted { get; set; }
        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [ForeignKey("BranchId")]
        public virtual Point Branch { get; set; }

        [ForeignKey("EntryBy")]
        public virtual User EntryByUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }
        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }
    }
}
