using Cloud_Storage.Models;

namespace Cloud_Storage.Classes.Handlers
{
    public static class AccessHandler
    {

        private static FolderAccessContext _folderAccess 
            = new FolderAccessContext();

        private static FileAccessContext _fileAccess 
            = new FileAccessContext();

        public static bool CheckFolderAccess(User user, string id)
        {

            return _folderAccess.FolderAccess
                .FirstOrDefault(x => x.user_id == user.Root && x.folder_id == id) != null;

        }

        public static bool CheckFileAccess(User user, string id)
        {

            return _fileAccess.FileAccess
                .FirstOrDefault(x => x.user_id == user.Root && x.file_id == id) != null;

        }

    }

}
