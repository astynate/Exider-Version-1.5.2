using Microsoft.EntityFrameworkCore;

namespace Cloud_Storage.Models
{
    public class FileAccessContext : DbContext
    {
        public DbSet<FileAccess> FileAccess { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class FileAccess
    {
        public int id { get; set; }
        public string? user_id { get; set; }
        public string? file_id { get; set; }

    }

}
