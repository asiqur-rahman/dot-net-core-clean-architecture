using Project.Core.Entities.Common.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities
{

    public class BaseEntity<T>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        [Display(Name = "Entry By")]
        public int? EntryBy { get; set; }

        [Display(Name = "Entry Date"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime? EntryDate { get; set; }

        [Display(Name = "Update By")]
        public int? UpdateBy { get; set; }

        [Display(Name = "Update Date"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "Approved By")]
        public int? ApprovedBy { get; set; }

        [Display(Name = "Approved Date"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-mm-dd}")]
        public DateTime? ApprovedDate { get; set; }

        [ForeignKey("EntryBy")]
        public virtual User Maker { get; set; }

        [ForeignKey("UpdateBy")]
        public virtual User Modifier { get; set; }

        [ForeignKey("ApprovedBy")]
        public virtual User Checker { get; set; }

        //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
