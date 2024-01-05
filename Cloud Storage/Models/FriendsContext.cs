// Part with directives
using Microsoft.EntityFrameworkCore;

// Namespaces part
namespace Cloud_Storage.Models
{

    // Create Friends Context (Using in database)
    public class FriendsContext : DbContext
    {

        public DbSet<Friend> Friends { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    // Create Requests Context (Using in database)
    public class RequestsContext : DbContext
    {

        public DbSet<Friend> FriendRequests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    // Class Friend (Using in database context)
    public class Friend
    {

        public int id { get; set; }
        public string? user_id { get; set; }
        public string? friend_id { get; set; }

    }

}
