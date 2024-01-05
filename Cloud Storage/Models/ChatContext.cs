using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Cloud_Storage.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Storage.Models
{

    public class ChatContext : DbContext
    {
        public DbSet<Chat> Chats { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class Chat
    {

        public int id { get; set; }
        public string? chat_id { get; set; }
        public string? name { get; set; }
        public string? admin_id { get; set; }

    }

    public class ChatView : Chat
    {
        public byte[]? avatar { get; set; }
        public Message? lastMessage { get; set; }
        public UserModel[]? users { get; set; }

        public ChatView() { }
        public ChatView(Chat chat, User user)
        {

            if (chat != null)
            {

                MessageContext mc = new MessageContext();

                chat_id = chat.chat_id;
                name = chat.name;
                admin_id = chat.admin_id;
                avatar = File.ReadAllBytes(Options.Path + "/__groups__/" + chat_id);

                avatar = Compression
                    .ResizeImageWidth(avatar, 100);

                lastMessage = mc.Messages
                    .Last(x => x.chat_id == chat_id);

            }

        }

    }

    public class ChatsModel
    {

        public ChatView[]? chats { get; set; }
        public DialogueView[]? dialogues { get; set; }
        public UserModel[]? users { get; set; }

        public ChatsModel(string userId)
        {

            dialogues = Database
                .GetDialoguesByRootId(userId);

            chats = Database
                .GetChatsByRootId(userId);

            users = Database
                .GetChatsParticipansByRootId(userId, chats);

        }

        public ChatsModel(ChatView[]? chats, UserModel[]? users)
        {
            this.chats = chats;
            this.users = users;
        }

    }

    public class ParticipantsContext : DbContext
    {
        public DbSet<Participant> Participants { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class Participant
    {

        public int id { get; set; }
        public string? chat_id { get; set; }
        public string? user_id { get; set; }

    }

}
