using Microsoft.EntityFrameworkCore;

namespace Cloud_Storage.Models
{

    // Create User Context (Using in Database)
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    // Create User (Using in database context)
    public class User
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Root { get; set; }
        public int StorageSpace { get; set; }

    }

    // Create User Model (Using in View)
    public class UserModel : User
    {
        public byte[]? Header { get; set; }
        public byte[]? Avatar { get; set; }

        public UserModel() { }

        public UserModel(User user, byte[] header, byte[]? avatar)
        {

            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Nickname = user.Nickname;
            Email = user.Email;
            Root = user.Root;
            StorageSpace = user.StorageSpace;
            Header = header;
            Avatar = avatar;

        }

    }

}
