using api.Context;
using api.Entities;

using Microsoft.EntityFrameworkCore;

using System.Data;
using System.Data.SqlClient;

namespace api.Repository
{
    public class PostRepository : IPostRepository
    {

        private readonly AppDBContext _context;
        string stringConnection = "";
        public PostRepository(AppDBContext context)
        {
            _context = context;
        }

        public PostRepository(IConfiguration configuration, bool hub)
        {
            _context = new AppDBContext(configuration);
        }

        public async Task CreatePost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(Guid id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Post>> GetAllPost()
        {
            _context.ChangeTracker.Clear();
            var posts = await _context.Posts.Where(p => p.Enabled == true).ToListAsync();
            return posts;
        }

        public async Task<Post?> GetPostById(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<bool> UpdatePost(Post post)
        {
            try
            {
                _context.Entry(post).State.Equals(EntityState.Modified);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException) when (!PostExists(post.Id))
            {
                return false;
            }
        }

        public bool PostExists(Guid id)
        {
            return _context.Posts.Any(p => p.Id == id);
        }

        public bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(p => p.Id == id);
        }

        public async Task<List<Post>> GetAllPostSql()
        {
            var query = "SELECT Id, Title, Content, CategoryId, CreateAt, UpdateAt, Enabled FROM Posts";

            using (var connection = new SqlConnection(stringConnection))
            {
                try
                {
                    connection.Open();
                    using(var cmd = new SqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var posts = new List<Post>();
                            while (reader.Read())
                            {
                                var post = new Post();
                                post.Id = reader.GetGuid(0);
                                post.Title = reader.GetString(1);
                                post.Content = reader.GetString(2);
                                post.CategoryId = reader.GetGuid(3);
                                post.CreateAt = reader.GetDateTime(4);
                                post.UpdateAt = reader.GetDateTime(5);
                                post.Enabled = reader.GetBoolean(6);
                                posts.Add(post);
                            }
                            return posts;
                        }
                    }
                     
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
