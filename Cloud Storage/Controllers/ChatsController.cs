using Cloud_Storage.Classes;
using Cloud_Storage.Classes.ChainProcessing;
using Cloud_Storage.Classes.Data;
using Cloud_Storage.Classes.Handlers;
using Cloud_Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cloud_Storage.Controllers
{
    public class ChatsController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index(string? id)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            UserModel? currentUser = Database.GetUserByEmail(Email);

            currentUser.Header = null;
            currentUser.Avatar = Compression
                .ResizeImageWidth(currentUser.Avatar, 100);

            ViewBag.currentUser = currentUser;

            ChatsModel? chatsModel = 
                new ChatsModel(currentUser.Root);

            if (chatsModel.chats != null && chatsModel.chats.Length > 0)
            {

                chatsModel.chats = chatsModel.chats
                    .Where(s => s.lastMessage != null)
                    .OrderBy(s => DateTime.Parse(s.lastMessage.time))
                    .Reverse()
                    .ToArray();

                foreach (ChatView chat in chatsModel.chats)
                {

                    chat.lastMessage.time = DateHandler
                        .ConvertToUSATimeFormat(chat.lastMessage.time);

                }

            }

            ViewBag.ActiveButton = "chats";
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = 0;

            //ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / currentUser.StorageSpace * 100);

            string userAgent = Request.Headers["User-Agent"].ToString();
            bool isMobile = userAgent.Contains("Mobi");

            if (isMobile)
            {
                return View("IndexMobile", chatsModel);
            }
            else
            {
                return View(chatsModel);
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateGroup(string[]? users)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            users = JsonConvert.DeserializeObject<string[]>(users[0]);

            if (users == null || users.Length < 2)
                return BadRequest();

            UserModel userModel = Database
                .GetUserByEmail(Email);

            string id = "G-" + Database.GenerateUniqueID("Chats", "chat_id");
            string group_name = Database.GetUserByRootId(users[0]).Name + ", " + Database.GetUserByRootId(users[1]).Name;

            using (var db = new ChatContext())
            {

                if (db.Chats.FirstOrDefault(x => x.name == group_name && x.admin_id == userModel.Root) == null)
                {

                    Chat chat = new Chat()
                    {

                        chat_id = id,
                        name = group_name,
                        admin_id = userModel.Root

                    };

                    System.IO.File.Copy(Options.Path + "/group_default_logo.png", Options.Path + "/__groups__/" + id, true);

                    db.Add(chat);
                    db.SaveChanges();

                }

            }

            using (var db = new ParticipantsContext())
            {

                Participant admin = new Participant()
                {

                    chat_id = id,
                    user_id = userModel.Root

                };

                db.Add(admin);

                foreach (var friend in users)
                {

                    Participant participant = new Participant()
                    {

                        chat_id = id,
                        user_id = friend

                    };

                    db.Add(participant);

                }

                db.SaveChanges();
            }

            using (var db = new MessageContext())
            {

                Message message = new Message()
                {

                    time = DateTime.Now.ToString(),
                    text = "The group was created",
                    chat_id = id,
                    user_id = userModel.Root,
                    reply = null,
                    type = "group"

                };

                db.Add(message);
                db.SaveChanges();

            }

            return Ok();

        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateDialogue(string userId)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            UserModel currentUser = Database
                .GetUserByEmail(Email);

            if (string.IsNullOrEmpty(userId))
                return StatusCode(400);

            if (ChatCOR.isChatExist(userId, currentUser.Root))
                return StatusCode(455);

            UserModel invitedUser = Database
                .GetUserByRootId(userId);

            using (DialogueContext db = new DialogueContext())
            {

                Dialogue dialogue = new Dialogue()
                {
                    chat_id = "D-" + Database.GenerateUniqueID("Dialogues", "chat_id"),
                    admin_id = currentUser.Root,
                    user_id = userId,
                };

                db.Add(dialogue);
                db.SaveChanges();

                SendInviteMessage(invitedUser, currentUser, dialogue.chat_id);

            }

            return RedirectToAction("index");

        }

        [HttpGet]
        [Authorize]
        public IActionResult Friends()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);
            var User = Database.GetUserByEmail(Email);

            UserModel[] friends = Database.GetFriendsByRootId(User.Root);

            return PartialView("_UserList", friends);

        }

        [HttpGet]
        [Authorize]
        public IActionResult ChatList()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);
            var User = Database.GetUserByEmail(Email);

            if (User.Root != null)
            {

                DialogueView[] chats = Database
                    .GetDialoguesByRootId(User.Root);

                return Ok(JsonConvert.SerializeObject(chats));

            }

            return StatusCode(400);

        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteMessage(int message_id)
        {

            using (MessageContext db = new MessageContext())
            {

                Message? message = db.Messages
                    .FirstOrDefault(x => x.id == message_id);

                if (message != null)
                {

                    db.Messages.Remove(message);
                    db.SaveChanges();

                    return Ok();

                }

            }

            return BadRequest();

        }

        [HttpPost]
        [Authorize]
        public IActionResult Users(string prefix)
        {

            UserModel[] friends = Database.GetUsersByPrefix(prefix);

            return PartialView("_UserList", friends);

        }

        [HttpGet]
        [Authorize]
        public string? Chat(string? id)
        {

            try
            {
                
                if (id != null) {

                    Message[]? messages = Database
                        .GetMessagesById(id);

                    return JsonConvert.SerializeObject(messages);

                }

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            return null;

        }

        private void SendInviteMessage(User invitedUser, User user, string id)
        {

            using (var db = new MessageContext())
            {

                Message message = new Message()
                {

                    time = DateTime.Now.ToString(),
                    text = invitedUser.Root,
                    chat_id = id,
                    user_id = user.Root,
                    reply = null,
                    type = "invite"

                };

                db.Add(message);
                db.SaveChanges();

            }

        }

    }

}
