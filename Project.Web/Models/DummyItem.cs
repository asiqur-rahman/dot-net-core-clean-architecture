using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models
{
    public class DummyItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}