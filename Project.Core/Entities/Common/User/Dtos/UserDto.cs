using Project.Common.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Entities.Helper;

namespace Project.Core.Entities.Common.User.Dtos
{
    public class UserDto
    {
        [Display(Name = "User ID")]
        public int UserId { get; set; }
        [Display(Name = "Parent Agent")]
        public int? ParentAgentId { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        [MaxLength(150, ErrorMessage = "{0} can have a max of {1} characters")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [MaxLength(50, ErrorMessage = "{0} can have a max of {1} characters")]
        public string UserName { get; set; }
        [Display(Name = "Role")]
        [Range(1, int.MaxValue, ErrorMessage = "User Role is Required!")]
        public int RoleId { get; set; }
        //[Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Display(Name = "NID")]
        public int? Nid { get; set; }
        [Display(Name = "TIN")]
        public int? Tin { get; set; }
        [StringLength(50)]
        [Display(Name = "Trade License")]
        public string TradeLicense { get; set; }

        [Display(Name = "Finger Print")]
        [MaxLength]
        public string FingerPrint { get; set; }

        [Required]
        [Display(Name = "Branch")]
        [Range(1, int.MaxValue, ErrorMessage = "Branch is Required!")]
        public int BranchId { get; set; }

        [Display(Name = "Emp. ID")]
        [MaxLength(50, ErrorMessage = "{0} can have a max of {1} characters")]
        public string EmployeeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Vacation { get; set; }
        [Display(Name = "Status")]
        public bool ActiveStatus { get; set; } = true;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        public string CurrentPassword { get; set; }
        public string PreviousPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("UserPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }

        [Display(Name = "Permitted Branch")]
        public string[] PermittedBranch { get; set; }
        [NotMapped] public int[] PermittedBranchData { get; set; }

        public string BranchIdList { get; set; }

        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }

        public string Designation { get; set; }

        [Display(Name = "User Branch")]
        public string UserBranch { get; set; }
        public int Id { get; set; }
        public string Password { get; set; }
        public int? EmpDesignation { get; set; }
        public bool IsAllBranchPermitted { get; set; }
        public bool IsBranchAllAgentPermitted { get; set; }
        [NotMapped] public ImageFile ProfilePictureFile { get; set; }
        public string ProfilePicture { get; set; }
        [NotMapped] public ImageFile SignatureImageFile { get; set; }
        public string SignatureImage { get; set; }
        public string PermittedBranchList { get; set; }
        [NotMapped] public int NumberOfFingure { get; set; }
        //[NotMapped] public IEnumerable<UserFingerPrint> VmFingerPrints { set; get; }
        public string[] VmFingerPosition { get; set; }
        public string[] VmFingerValue { get; set; }
        [Display(Name = "MAC Address"), StringLength(35, ErrorMessage = "{0} can have a max of {1} characters"), Required]
        public string MAC { get; set; } = "00:00:00:00:00:00";
        [NotMapped] public string PreBranch { get; set; }
        [NotMapped] public string PreParentAgent { get; set; }
        [NotMapped] public string PrePermittedBranch { get; set; }
    }

}
