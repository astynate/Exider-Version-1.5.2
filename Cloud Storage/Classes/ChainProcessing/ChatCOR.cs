using Cloud_Storage.Models;

namespace Cloud_Storage.Classes.ChainProcessing
{
    public class ChatCOR
    {

        internal static bool isChatExist(string firstUserId, string secondUserId)
        {

            if (firstUserId != null || secondUserId != null)
            {

                DialogueContext db = new DialogueContext();

                return db.Dialogues.FirstOrDefault(x => (x.user_id == firstUserId && x.admin_id == secondUserId) || 
                    (x.user_id == secondUserId && x.admin_id == firstUserId)) != null;

            }

            return true;

        }

    }

}
