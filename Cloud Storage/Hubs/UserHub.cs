using Cloud_Storage.Classes;
using Cloud_Storage.Controllers;
using Cloud_Storage.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Cloud_Storage.Hubs
{
    public class UserHub : Hub
    {
        public async Task SendMessage(string JwtToken, string chatId, string message, string replyMessageId)
        {

            var Email = JwtHandler.GetEmailByToken(JwtToken);
            var userObject = Database.GetUserByEmail(Email);

            userObject.Header = null;

            if (chatId != null && !string.IsNullOrEmpty(message))
            {

                using (var db = new MessageContext())
                {

                    var messageModel = new Message()
                    {

                        time = DateTime.Now.ToString(),
                        text = message,
                        chat_id = chatId,
                        user_id = userObject.Root,
                        reply = $"{replyMessageId} {Database.GetUserByMessageId(Convert.ToInt32(replyMessageId)).Root}",
                        type = (replyMessageId == null) ? "default" : "reply"

                    };

                    try
                    {

                        var time = (messageModel.time.Split(" ")[1] != null) ? messageModel.time.Split(" ")[1].Split(":")[0] + ":" +
                        messageModel.time.Split(" ")[1].Split(":")[1] : " ";

                        UserModel? replyUserObject;
                        Message? replyMessage;

                        if (replyMessageId != null)
                        {

                            replyUserObject = Database
                                .GetUserByMessageId(Convert.ToInt32(replyMessageId));

                            replyMessage = db.Messages
                                .FirstOrDefault(x => x.id == Convert.ToInt32(replyMessageId));

                        }

                        db.Add(messageModel);
                        db.SaveChanges();

                        string messageJson = System.Text.Json
                            .JsonSerializer.Serialize(messageModel);

                        await Clients.Group(chatId)
                            .SendAsync("MessageToGroup", messageJson);

                    }

                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);

                    }

                }

            }

        }

        public async Task SendForwardMessage(string JwtToken, string chatId, string messageId)
        {

            var Email = JwtHandler.GetEmailByToken(JwtToken);
            var userObject = Database.GetUserByEmail(Email);

            try
            {

                using (MessageContext db = new MessageContext())
                {

                    Message? inputMessage = db.Messages
                        .FirstOrDefault(x => x.id == Convert.ToInt32(messageId));

                    if (new DialogueContext().Dialogues.FirstOrDefault(x => x.chat_id == chatId) == null)
                        return;

                    if (inputMessage != null)
                    {

                        var sender = Database.GetUserByRootId(inputMessage.user_id);
                        var time = (inputMessage.time.Split(" ")[1] != null) ? inputMessage.time.Split(" ")[1].Split(":")[0] + ":" +
                            inputMessage.time.Split(" ")[1].Split(":")[1] : " ";

                        Message forwardMessage = new Message()
                        {

                            chat_id = chatId,
                            type = "forward",
                            text = inputMessage.text,
                            user_id = userObject.Root,
                            reply = $"{sender.Name} {sender.Surname}",
                            time = DateTime.Now.ToString()

                        };

                        db.Messages.Add(forwardMessage);
                        db.SaveChanges();

                        await Clients.Group(chatId)
                            .SendAsync("MessageToGroup", JsonConvert.SerializeObject(forwardMessage));

                    }

                }

            }

            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync(ex.Message);

            }

        }

        public async Task DeleteMessage(string chatId, string messageId)
        {

            if (messageId != null)
            {

                try
                {

                    using (MessageContext db = new MessageContext())
                    {

                        db.Remove(db.Messages.FirstOrDefault(x => x.id == Convert.ToInt32(messageId)));
                        db.SaveChanges();

                    };

                    await Clients.Group(chatId)
                        .SendAsync("DeleteMessage", messageId);

                } 
                
                catch (Exception ex)
                {

                    await Console.Out
                        .WriteLineAsync(ex.Message);

                }

            }

        }
       
        public async Task EditMessage(string? text, string? mesageId)
        {

            MessageContext mc = new MessageContext();

            Message? message = mc.Messages
                .FirstOrDefault(x => x.id == Convert.ToInt64(mesageId));

            if (message != null && text != null)
            {

                message.text = text;
                message.is_edited = 1;

                mc.Update(message);
                mc.SaveChanges();

                string messageJson = JsonConvert.SerializeObject(message);
                await Clients.Group(message.chat_id).SendAsync("EditMessage", messageJson);

            }

        }
        
        public async Task TypingNotification(string? email, string? chatId)
        {

            User? user = Database
                .GetUserByEmail(email);

            await Clients.GroupExcept(chatId, Context.ConnectionId).SendAsync("TypingNotification", user.Name);

        }

        public async Task ChangeViewState(string? mesageId)
        {

            MessageContext mc = new MessageContext();

            Message? message = mc.Messages
                .FirstOrDefault(x => x.id == Convert.ToInt64(mesageId));

            if (message != null)
            {

                message.is_viewed = 1;

                mc.Update(message);
                mc.SaveChanges();

                string messageJson = JsonConvert.SerializeObject(message);
                await Clients.Group(message.chat_id).SendAsync("ChangeViewState", messageJson);

            }


        }

        public async Task JoinChat(string? chatId)
        {

            if (chatId != null)
            {

                await Groups.AddToGroupAsync
                    (Context.ConnectionId, chatId.ToString());

            }

        }

        public async Task LeaveChat(string chatId)
        {

            await Groups.RemoveFromGroupAsync
                (Context.ConnectionId, chatId);

        }

    }
}
