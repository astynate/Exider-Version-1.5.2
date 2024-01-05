using Microsoft.EntityFrameworkCore;

namespace Cloud_Storage.Models
{
    public class FolderAccessContext : DbContext
    {
        public DbSet<FolderAccess> FolderAccess { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class FolderAccess
    {

        public int id { get; set; }
        public string? user_id { get; set; }
        public string? folder_id { get; set; }

    }

}
