using Cloud_Storage.Classes;
using Cloud_Storage.Models;

namespace Cloud_Storage.Controllers
{
    public static class Storage
    {

        private static readonly FolderContext _folderContext = new FolderContext();

        public static void DeleteFileById(string id)
        {

            using (var db = new FileContext())
            {

                var recordToDelete = db.Files
                    .FirstOrDefault(x => x.id == id);

                File.Delete(Options.Path + "/" + recordToDelete.id);
                db.Files.Remove(recordToDelete);

                db.SaveChanges();

            }

            using (var db = new FileAccessContext())
            {

                var fileAccess = db.FileAccess
                    .Where(x => x.file_id == id);

                foreach (var file in fileAccess)
                {
                    db.Remove(file);
                }

                db.SaveChanges();

            }

        }
        public static void DeleteFolder(string input_id)
        {

            using (var db = new FolderContext())
            {

                var fodler = db.Folders
                    .FirstOrDefault(x => x.id == input_id);

                if (fodler != null)
                {
                    db.Folders.Remove(fodler);
                    db.SaveChanges();
                }

            }

            using (var db = new FolderAccessContext())
            {

                var folderAccess = db.FolderAccess
                    .Where(x => x.folder_id == input_id);

                foreach (var folder in folderAccess)
                {

                    if (folder != null)
                    {
                        db.Remove(folder);
                    }

                    db.SaveChanges();

                }

            }

            using (var db = new FileContext())
            {

                var filesToDelete = db.Files
                    .Where(x => x.folder_id == input_id).ToList();

                foreach (var file in filesToDelete)
                {

                    if (file.id != null) 
                        DeleteFileById(file.id);

                }

            }

        }
        public static void DeleteFolderById(string input_id)
        {

            DeleteFolder(input_id);

            using (var data_base = new FolderContext())
            {

                Folder[] folder = Database.GetFoldersByFolderId(input_id);

                for (int i = 0; i < folder.Length; i++)
                {

                    if (folder[i].id != null)
                        DeleteFolderById(folder[i].id);

                }

            }

        }
        public static double GetUserOccupiedSpace(string email)
        {

            User user = Database.GetUserByEmail(email);
            long result = 0;
            
            CalculateFolderSpace(user.Root, ref result);

            return Convert.ToDouble(result / Math.Pow(1024, 2));

        }
        private static long CalculateFolderFileSpace(string id)
        {

            long result = 0;

            FileModel[] files = Database.GetFilesByFolderId(id);

            foreach (var file in files)
            {

                FileInfo fileInfo = new FileInfo(Options.Path + file.id);
                result += fileInfo.Length;

            }

            return result;

        }
        private static void CalculateFolderSpace(string id, ref long result)
        {

            result += CalculateFolderFileSpace(id);

            using (var data_base = new FolderContext())
            {

                Folder[] folder = Database.GetFoldersByFolderId(id);

                for (int i = 0; i < folder.Length; i++)
                {

                    if (folder[i].id != null)
                    {

                        CalculateFolderSpace(folder[i].id, ref result);

                    }

                }

            }

            return;

        }
        public static Folder[] GetCurrentPathByFolderId(string id)
        {

            Stack<Folder> folders = new Stack<Folder>();
            Folder? folder = _folderContext.Folders.FirstOrDefault(x => x.id == id);

            while (folder != null)
            {

                folders.Push(folder);
                folder = _folderContext.Folders.FirstOrDefault(x => x.id == folder.folder_id);

            }

            foreach (Folder f in folders)
            {

                Console.WriteLine(f.name);

            }

            return folders.ToArray();

        }

    }

}
