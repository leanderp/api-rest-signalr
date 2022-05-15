using System.ComponentModel.DataAnnotations;

namespace api.Entities.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }
}
