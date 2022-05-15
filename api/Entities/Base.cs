namespace api.Entities
{
    public class Base
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
