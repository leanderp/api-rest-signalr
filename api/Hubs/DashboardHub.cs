
using api.Repository;

using Microsoft.AspNetCore.SignalR;

namespace api.Hubs
{
    public class DashboardHub : Hub
    {
        private readonly PostRepository _repository;
        public DashboardHub(IConfiguration configuration)
        {
            _repository = new PostRepository(configuration, true);
        }

        public async Task SendPosts()
        {
            try
            {
                if (Clients is not null)
                {
                    var posts = await _repository.GetAllPost();
                    await Clients.All.SendAsync("ReceivedPosts", posts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
