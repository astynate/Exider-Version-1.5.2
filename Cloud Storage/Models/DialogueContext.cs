using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Storage.Models
{
    public class DialogueContext : DbContext
    {
        public DbSet<Dialogue> Dialogues { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=user_data.db");

    }

    public class Dialogue
    {

        public int id { get; set; }
        public string? chat_id { get; set; }
        public string? user_id { get; set; }
        public string? admin_id { get; set; }

    }

    public class DialogueView : Dialogue
    {

        public Message? lastMessage { get; set; }

        public DialogueView(string userId, Dialogue dialogue)
        {
            
            if (string.IsNullOrEmpty(userId) == false)
            {

                try
                {

                    id = dialogue.id;
                    chat_id = dialogue.chat_id;
                    user_id = dialogue.user_id;
                    admin_id = dialogue.admin_id;

                    MessageContext mc = new MessageContext();

                    lastMessage = mc.Messages
                        .OrderBy(x => x.id)
                        .Last(x => x.chat_id == dialogue.chat_id);

                    UserModel userModel = Database
                        .GetUserByRootId((dialogue.user_id == userId) ? userId : dialogue.admin_id);

                }

                catch (Exception ex) 
                {
                
                    Console.WriteLine(ex.ToString());

                }

            }

        }

    }

}