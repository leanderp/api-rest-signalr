using api.Entities;

namespace api.Repository
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPost();
        Task<List<Post>> GetAllPostSql();
        Task<Post?> GetPostById(Guid id);
        Task CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task DeletePost(Guid id);
        bool PostExists(Guid id);
        bool CategoryExists(Guid id);
    }
}
