using api.Entities.DTOs;

namespace api.Entities
{
    public class Post : Base
    {
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string Content { get; set; }
    }
}
