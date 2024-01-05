using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cloud_Storage.Models
{

    public class MessageContext : DbContext
    {
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class Message
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? time { get; set; }
        public string? text { get; set; }
        public string? chat_id { get; set; }
        public string? user_id { get; set; }
        public string? reply { get; set; }
        public string? type { get; set; }
        public int? is_edited { get; set; }
        public int? is_viewed { get; set; }

    }

    public class MessageView : Message
    {
        public UserModel? user { get; set; }
        public MessageView? messageReply { get; set; }
        public MessageView() { }
        public MessageView(Message message)
        {

            if (message != null)
            {

                id = message.id;
                time = (message.time.Split(" ")[1] != null) ? message.time.Split(" ")[1].Split(":")[0] + ":" + message.time.Split(" ")[1].Split(":")[1] : " ";
                text = message.text;
                chat_id = message.chat_id;
                user_id = message.user_id;
                reply = message.reply;
                type = message.type;
                is_edited = message.is_edited;
                is_viewed = message.is_viewed;
                user = Database.GetUserByRootId(message.user_id);

                user.Avatar = Compression.ResizeImageWidth(user.Avatar, 100);
                user.Header = null;

                if (message.type == "reply")
                {

                    MessageContext context = new MessageContext();

                    int replyMessageId = Convert
                        .ToInt32(message.reply.Split(" ")[0]);

                    if (message.type != null && message.reply.Split(" ")[0] != null)
                    {

                        Message? messageObject = context.Messages
                            .FirstOrDefault(x => x.id == replyMessageId);

                        if (messageObject != null)
                        {

                            messageReply = new 
                                MessageView(messageObject);

                            messageReply.user.Avatar = Compression.ResizeImageWidth(messageReply.user.Avatar, 100);
                            messageReply.user.Header = null;

                        }

                    }

                }

            }

        }

    }

}
