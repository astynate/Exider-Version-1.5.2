// Part with directives
using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Handlers;
using Humanizer;
using Microsoft.EntityFrameworkCore;

// Namespaces part
namespace Cloud_Storage.Models
{
    
    // Create Folder Context (Using in database)
    public class FolderContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    // Create FolderModel (Using in database context)
    public class Folder
    {

        public string? id { get; set; }
        public string? name { get; set; }
        public string? date { get; set; }
        public string? folder_id { get; set; }

        public Folder() { }

        public Folder(User user, string name, string id)
        {

            this.id = Database.GenerateUniqueID("folders", "folder_id");
            this.name = name;
            date = DateTime.Now.ToString();
            folder_id = id;

            using (FolderAccessContext db = new FolderAccessContext())
            {

                FolderAccess folderAccess = new FolderAccess()
                {

                    user_id = user.Root,
                    folder_id = this.id

                };

                db.Add(folderAccess);
                db.SaveChanges();

            }

        }

    }

    public class FolderView : Folder
    {

        public string? time { get; set; }

        public FolderView(Folder obj)
        {

            id = obj.id;
            name = obj.name;
            date = DateHandler.ConvertToUSADateFormat(obj.date);
            time = DateHandler.ConvertToUSATimeFormat(obj.date);
            folder_id = obj.folder_id;

        }

    }

}
