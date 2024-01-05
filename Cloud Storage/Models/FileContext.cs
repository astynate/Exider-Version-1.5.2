using Cloud_Storage.Classes.Handlers;
using Cloud_Storage.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Cloud_Storage.Models
{

    public class FileContext : DbContext
    {

        public DbSet<FileModel> Files { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class FileModel
    {
        [JsonProperty]
        public string? id { get; set; }

        [JsonProperty]
        public string? name { get; set; }

        [JsonProperty]
        public string? type { get; set; }

        [JsonProperty]
        public string? date { get; set; }

        [JsonProperty]
        public string? folder_id { get; set; }

        public FileModel() 
        {

            date = DateTime.Now.ToString();

        }

        public FileModel(User user, params string[] args) 
        {

            if (args.Length >= 4) 
            {

                id = args[0];
                name = args[1];
                type = args[2];
                date = DateTime.Now.ToString();
                folder_id = args[3];

                using (FileAccessContext db = new FileAccessContext())
                {

                    FileAccess fileAccess = new FileAccess()
                    {

                        user_id = user.Root,
                        file_id = id

                    };

                    db.Add(fileAccess);
                    db.SaveChanges();

                }

            }

        }

    }

    public class FileView : FileModel
    {
        [JsonProperty]
        public string? full_name { get; set; }

        [JsonProperty]
        public byte[]? file { get; set; }

        [JsonProperty]
        public string? time { get; set; }

        public FileView() { }

        public FileView(User user, FileModel obj) : base(user)
        {

            using (var db = new FileContext())
            {

                FileModel? bufferFile = db.Files.FirstOrDefault(x => x.id == obj.id);

                id = obj.id;
                name = obj.name;
                type = obj.type;
                date = DateHandler.ConvertToUSADateFormat(obj.date);
                time = DateHandler.ConvertToUSATimeFormat(obj.date);
                folder_id = obj.folder_id;
                full_name = bufferFile.name;
                file = File.ReadAllBytes($"{Options.Path}/{bufferFile.id}");

            }

        }

    }

    public class FilesModel
    {
        public FileView[]? Files { get; set; }
        public FolderView[]? Folders { get; set; }
        public Folder[]? Path { get; set; }

    }

}
