using api.Entities;
using api.Hubs;

using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace api.SubscribeTableDependencies
{
    public class SubscribePostTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Post> tableDependency;
        private readonly DashboardHub _dashboarHub;
        public SubscribePostTableDependency(DashboardHub dashboarHub)
        {
            _dashboarHub = dashboarHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Post>(connectionString, "Posts");
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<Post> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                if (_dashboarHub is not null)
                {
                    Task.FromResult(_dashboarHub.SendPosts());
                }
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Post)}: SqlTableDependency error: {e.Error.Message}");
        }
    }
}
