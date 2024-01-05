using Microsoft.Data.Sqlite;
using Cloud_Storage.Models;
using Cloud_Storage.Controllers;
using FileAccess = Cloud_Storage.Models.FileAccess;
using Cloud_Storage.Classes.Handlers;
using Cloud_Storage.Classes.Data;

namespace Cloud_Storage.Classes
{
    public static class Database
    {
        public static UserModel GetUserByEmail(string? email)
        {

            try
            {

                using (UserContext db = new UserContext())
                {

                    User? userModel = db.Users.FirstOrDefault(x => x.Email == email);

                    if (userModel != null)
                    {

                        return new UserModel(
                            userModel,
                            File.ReadAllBytes(Options._userHeaderPath + userModel.Root),
                            File.ReadAllBytes(Options._userAvatarPath + userModel.Root)
                        );

                    }

                }

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            return new UserModel();

        }
        public static UserModel GetUserByRootId(string? id)
        {

            using (UserContext db = new UserContext())
            {

                User? userModel = db.Users.FirstOrDefault(x => x.Root == id);

                if (userModel != null)
                {

                    return new UserModel(
                        userModel,
                        File.ReadAllBytes(Options._userHeaderPath + userModel.Root),
                        File.ReadAllBytes(Options._userAvatarPath + userModel.Root)
                    );

                }

                return new UserModel();

            }

        }
        public static UserModel[] GetUsersByPrefix(string? prefix)
        {

            Stack<UserModel> userStack = new Stack<UserModel>();

            using (UserContext db = new UserContext())
            {

                if (prefix != null)
                {

                    User[] users = db.Users.Where(x => x.Nickname.ToLower().Contains(prefix.ToLower()) ||
                    x.Name.ToLower().Contains(prefix.ToLower()) || x.Surname.ToLower().Contains(prefix.ToLower()) ||
                    (x.Name.ToLower() + " " + x.Surname.ToLower()).Contains(prefix.ToLower())).ToArray();

                    foreach (var user in users)
                    {

                        userStack.Push(new UserModel(user, File.ReadAllBytes(Options._userHeaderPath + user.Root),
                        File.ReadAllBytes(Options._userAvatarPath + user.Root)));

                    }

                }

            }

            return userStack.ToArray();

        }
        public static FileView[] GetFilesByFolderId(string id)
        {

            using (FileContext db = new FileContext())
            {

                FileModel[]? files = db.Files.Where(x => x.folder_id == id).ToArray();
                Stack<FileView> result = new Stack<FileView>();

                foreach (FileModel file in files)
                {

                    result.Push(new FileView()
                    {

                        id = file.id,
                        full_name = file.name,
                        name = file.name,
                        type = file.type,
                        time = DateHandler.ConvertToUSATimeFormat(file.date),
                        date = DateHandler.ConvertToUSADateFormat(file.date),
                        folder_id = file.folder_id,
                        file = File.ReadAllBytes(Options.Path + "/" + file.id)

                    });

                }

                return result.ToArray();

            }

        }
        public static FileView[] GetAllFilesByEmail(string email)
        {

            User user = GetUserByEmail(email);
            Stack<FileView> files = new Stack<FileView>();

            using (FileContext db = new FileContext())
            {

                FileAccessContext database = new FileAccessContext();
                FileAccess[] fileAccess = database.FileAccess.Where(x => x.user_id == user.Root).ToArray();

                foreach (FileAccess file in fileAccess)
                {

                    FileModel? bufferFile = db.Files.FirstOrDefault(x => x.id == file.file_id);

                    if (bufferFile != null && Options.ImageTypes.Contains(bufferFile.type))
                    {

                        files.Push(new FileView()
                        {

                            id = bufferFile.id,
                            full_name = bufferFile.name,
                            name = bufferFile.name,
                            type = bufferFile.type,
                            file = File.ReadAllBytes(Options.Path + bufferFile.id)

                        });

                    }

                }

            }

            return files.ToArray();

        }
        public static UserModel[] GetFriendsByRootId(string id)
        {

            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {

                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM Friends", connection);

                UserModel[] friends;
                Stack<UserModel> friends_stack = new Stack<UserModel>();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if ((string)reader.GetValue(1) == id)
                            {

                                UserModel User = GetUserByRootId((string)reader.GetValue(2));

                                User.Avatar = Compression
                                    .ResizeImageWidth(User.Avatar, 100);

                                friends_stack.Push(User);

                            }

                            else if ((string)reader.GetValue(2) == id)
                            {

                                UserModel User = GetUserByRootId((string)reader.GetValue(1));


                                User.Avatar = Compression
                                    .ResizeImageWidth(User.Avatar, 100);

                                friends_stack.Push(User);

                            }

                        }

                        int index = 0;
                        friends = new UserModel[friends_stack.Count];

                        while (friends_stack.Count > 0)
                        {

                            friends[index] = friends_stack.Pop();
                            index++;

                        }

                        return friends;

                    }
                }
            }

            return new UserModel[0];

        }
        public static UserModel[] GetRequestsByRootId(string id)
        {

            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {

                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM FriendRequests", connection);

                UserModel[] friends;
                Stack<UserModel> friends_stack = new Stack<UserModel>();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if ((string)reader.GetValue(1) == id)
                            {

                                UserModel User = GetUserByRootId((string)reader.GetValue(2));
                                friends_stack.Push(User);

                            }

                        }

                        int index = 0;
                        friends = new UserModel[friends_stack.Count];

                        while (friends_stack.Count > 0)
                        {

                            friends[index] = friends_stack.Pop();
                            index++;

                        }

                        return friends;

                    }
                }
            }

            return new UserModel[0];

        }
        public static UserModel[] GetInvitationsByRootId(string id)
        {

            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {

                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM FriendRequests", connection);

                UserModel[] friends;
                Stack<UserModel> friends_stack = new Stack<UserModel>();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            if ((string)reader.GetValue(2) == id)
                            {

                                UserModel User = GetUserByRootId((string)reader.GetValue(1));
                                friends_stack.Push(User);

                            }

                        }

                        int index = 0;
                        friends = new UserModel[friends_stack.Count];

                        while (friends_stack.Count > 0)
                        {

                            friends[index] = friends_stack.Pop();
                            index++;

                        }

                        return friends;

                    }
                }
            }

            return new UserModel[0];

        }
        public static FolderView[] GetFoldersByFolderId(string id)
        {

            Stack<FolderView> folders_stack = new Stack<FolderView>();

            using (FolderContext db = new FolderContext())
            {

                var folders = db.Folders
                    .Where(x => x.folder_id == id).ToArray();

                foreach (var folder in folders)
                {

                    folders_stack.Push(new FolderView(folder));

                }

            }

            return folders_stack.ToArray();

        }
        public static List<string> GetChatsIdByRootId(string? id)
        {

            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {

                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM Participants", connection);

                List<string> chats_id = new List<string>();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            if (reader.GetString(2) == id)
                            {

                                chats_id.Add(reader.GetString(1));

                            }

                        }

                    }
                }

                return chats_id;

            }

        }
        public static ChatView[] GetChatsByRootId(string id)
        {

            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {

                connection.Open();
                SqliteCommand command = new SqliteCommand("SELECT * FROM Chats", connection);

                Stack<ChatView> chat_stack = new Stack<ChatView>();
                List<string> chats_id = GetChatsIdByRootId(id);

                MessageContext mc = new MessageContext();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            var chat_id = reader.GetString(1);

                            if (chats_id.Contains(chat_id))
                            {

                                chats_id.Remove(chat_id);

                                Console.WriteLine(chat_id);

                                ChatView chat = new ChatView()
                                {

                                    chat_id = chat_id,
                                    name = reader.GetString(2),
                                    admin_id = reader.GetString(3),
                                    avatar = Compression.ResizeImageWidth(File.ReadAllBytes(Options.Path + "/__groups__/" + chat_id), 100),

                                    lastMessage = (mc.Messages.Count() > 0) ? mc.Messages.OrderBy(id => id)
                                                .Last(x => x.chat_id == chat_id) : null

                                };

                                chat_stack.Push(chat);

                            }

                        }

                    }

                }

                return chat_stack.ToArray();

            }

        }
        public static Message[]? GetMessagesById(string id)
        {

            try
            {

                using (MessageContext db = new MessageContext())
                {

                    Message[] inputMessages = db
                        .Messages.OrderByDescending(x => x.id)
                            .Where(x => x.chat_id == id).Take(30).ToArray();

                    return inputMessages.ToArray();

                }

            }

            catch (Exception exception) {

                Console.WriteLine(exception.Message);

            }

            return new Message[0];

        }
        public static UserModel GetUserByMessageId(int id)
        {

            try
            {

                using (MessageContext db = new MessageContext())
                {

                    Message? message = db.Messages.FirstOrDefault(x => x.id == id);

                    if (message != null)
                    {

                        return GetUserByRootId(message.user_id);

                    }

                }
    
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            return new UserModel();

        }
        public static UserModel[]? GetChatsParticipansByRootId(string userId, ChatView[] chats)
        {

            List<UserModel> users = new List<UserModel>();

            foreach (ChatView chat in chats)
            {

                using (ParticipantsContext db = new ParticipantsContext())
                {

                    Participant[]? participans = db.Participants
                        .Where(x => x.chat_id == chat.chat_id).ToArray();

                    if (participans.Length > 0)
                    {

                        foreach(Participant participant in participans)
                        {

                            UserModel user = GetUserByRootId
                                (participant.user_id);

                            if (users.Contains(user) == false) {

                                if (user.Avatar != null)
                                {

                                    user.Avatar = Compression
                                        .ResizeImageWidth(user.Avatar, 100);

                                }

                                user.Header = null;
                                users.Add(user);

                            }

                        }

                    }

                }

            }

            DialogueContext dc = new DialogueContext();

            Dialogue[]? dialogues = dc.Dialogues
                .Where(x => x.user_id == userId || x.admin_id == userId).ToArray();

            foreach (Dialogue dialogue in dialogues)
            {

                UserModel[] dialogueParticipans = new UserModel[2] {

                    GetUserByRootId(dialogue.user_id),
                    GetUserByRootId(dialogue.admin_id)

                };

                foreach (UserModel user in dialogueParticipans)
                {

                    if (users.Contains(user) == false)
                    {

                        if (user.Avatar != null)
                        {

                            user.Avatar = Compression
                                .ResizeImageWidth(user.Avatar, 100);

                        }

                        user.Header = null;
                        users.Add(user);

                    }

                }

            }

            return users.ToArray();

        }
        public static DialogueView[]? GetDialoguesByRootId(string? userId)
        {

            List<DialogueView> dialogues = new List<DialogueView>();
            
            if (string.IsNullOrEmpty(userId) == false)
            {

                using (DialogueContext db = new DialogueContext())
                {

                    Dialogue[] dialoguesBuffer = db.Dialogues
                                    .Where(x => x.user_id == userId || x.admin_id == userId)
                                    .ToArray();

                    Array.ForEach(dialoguesBuffer, 
                        x => dialogues.Add(new DialogueView(userId, x)));

                }

            }

            return dialogues.ToArray();

        }
        public static string[] GetLeftPanelDataByEmail(string email)
        {

            List<string> result = new List<string>();
            UserModel user = GetUserByEmail(email);

            FileAccessContext fileDB = new FileAccessContext();

            result.Add("0");
            result.Add(fileDB.FileAccess.Where(x => x.user_id == user.Root)
                .ToArray().Length.ToString());
            result.Add(GetAllFilesByEmail(email).Length.ToString());
            result.Add("0");
            result.Add(GetInvitationsByRootId(user.Root).Length.ToString());
            result.Add("0");

            return result.ToArray();

        }
        public static string GenerateUniqueID(string table, string column) {

            // Create Root Id Variable
            string UniqueRootId = "";

            // Create HEX Numbers Array
            char[] alphabet = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            // Create Random OBJ
            Random random = new Random();

            // Generate Root Id
            for (int i = 0; i < 12; i++)
            {

                // Generate Random Symbol Index
                int index = random.Next(alphabet.Length);

                // Add Symbol To Root Id
                UniqueRootId += alphabet[index];

            }

            // Check The Uniqueness Of The Id
            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {
                // Open SQLite Connection
                connection.Open();

                // Write Command 
                SqliteCommand command = new SqliteCommand($"SELECT COUNT(*) FROM {table} WHERE {column} = '{UniqueRootId}'", connection);

                // Get Count Entries
                int count = Convert.ToInt32(command.ExecuteScalar());

                // If Root Id not Unique
                if (count > 0)
                {
                    return GenerateUniqueRootID();
                }

            }

            // Return Unique Root Id
            return UniqueRootId;

        }
        public static string GenerateUniqueRootID()
        {

            // Return Unique Root Id
            return GenerateUniqueID("Users", "Root");

        }
        public static string GenerateUniqueFileID()
        {

            // Return Unique File Id
            return GenerateUniqueID("Files", "id");

        }

    }
}
