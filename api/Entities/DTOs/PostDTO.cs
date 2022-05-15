using System.ComponentModel.DataAnnotations;

namespace api.Entities.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public bool Enabled { get; set; }
    }
}
