using System.ComponentModel.DataAnnotations;

namespace api.Entities.DTOs
{
    public class InsertCategoryDTO
    {
        [Required]
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }
}
