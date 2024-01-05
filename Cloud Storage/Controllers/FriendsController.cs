using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Cloud_Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Storage.Controllers
{
    public class FriendsController : Controller
    {

        private readonly FriendsContext _firendContext = new FriendsContext();

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            ViewData["type"] = "friends";
            ViewData["root_id"] = Database.GetUserByEmail(Email).Root;
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            ViewBag.ActiveButton = "friends";

            UserModel[] friends = Database.GetFriendsByRootId(user.Root);

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return View(friends);

        }

        [HttpGet]
        [Authorize]
        public IActionResult Requests()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            ViewData["type"] = "friends";
            ViewData["root_id"] = Database.GetUserByEmail(Email).Root;
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            UserModel[] requests = Database.GetRequestsByRootId(user.Root);

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return View(requests);

        }

        [HttpGet]
        [Authorize]
        public IActionResult Invitations()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            ViewData["type"] = "friends";
            ViewData["root_id"] = Database.GetUserByEmail(Email).Root;
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            UserModel[] invitations = Database.GetInvitationsByRootId(user.Root);

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return View(invitations);

        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(string search_request)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);
            var user = Database.GetUserByEmail(Email);

            ViewData["type"] = "search";
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return View(Database.GetUsersByPrefix(search_request));

        }

        [HttpPost]
        [Authorize]
        public IActionResult AddFriend(string id, string option)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            if (Database.GetUserByRootId(id).Root == user.Root)
                return StatusCode(400);

            if (option == "friend")
            {

                using (FriendsContext data_base = new FriendsContext())
                {

                    Friend friend = new Friend()
                    {

                        user_id = Database.GetUserByEmail(Email).Root,
                        friend_id = id

                    };

                    data_base.Add(friend);
                    data_base.SaveChanges();

                }

                using (RequestsContext data_base = new RequestsContext())
                {

                    var friend = data_base.FriendRequests.FirstOrDefault(x => (x.user_id == Database.GetUserByEmail(Email).Root && x.friend_id == id) || (x.user_id == id && x.friend_id == Database.GetUserByEmail(Email).Root));

                    if (friend != null)
                    {

                        data_base.Remove(friend);
                        data_base.SaveChanges();

                    }

                }

            }

            else if (option == "request")
            {

                using (RequestsContext data_base = new RequestsContext())
                {

                    var get_request = data_base.FriendRequests.FirstOrDefault(x => (x.user_id == Database.GetUserByEmail(Email).Root && x.friend_id == id) || (x.user_id == id && x.friend_id == Database.GetUserByEmail(Email).Root));
                    var get_friend = _firendContext.Friends.FirstOrDefault(x => (x.user_id == Database.GetUserByEmail(Email).Root && x.friend_id == id) || (x.user_id == id && x.friend_id == Database.GetUserByEmail(Email).Root));

                    if (get_request == null && get_friend == null)
                    {

                        Friend friend = new Friend()
                        {

                            user_id = Database.GetUserByEmail(Email).Root,
                            friend_id = id

                        };

                        data_base.Add(friend);
                        data_base.SaveChanges();

                    }

                }

            }

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return RedirectToAction("index");

        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(string id, string option)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            if (option == "request")
            {

                if (id.Split(' ').Length > 1)
                {

                    string? UserId = id.Split(' ')[0];
                    string? FriendId = id.Split(' ')[1];

                    using (RequestsContext data_base = new RequestsContext())
                    {

                        var friend = data_base.FriendRequests.FirstOrDefault(x => (x.user_id == UserId && x.friend_id == FriendId) || (x.user_id == FriendId && x.friend_id == UserId));

                        if (friend != null)
                        {

                            data_base.Remove(friend);
                            data_base.SaveChanges();

                        }

                    }

                }

            }

            else if (option == "friend")
            {

                if (id.Split(' ').Length > 1)
                {

                    string? UserId = id.Split(' ')[0];
                    string? FriendId = id.Split(' ')[1];

                    using (FriendsContext data_base = new FriendsContext())
                    {

                        var friend = data_base.Friends.FirstOrDefault(x => (x.user_id == UserId && x.friend_id == FriendId) || (x.user_id == FriendId && x.friend_id == UserId));

                        if (friend != null)
                        {

                            data_base.Remove(friend);
                            data_base.SaveChanges();

                        }

                    }

                }

            }

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return RedirectToAction("Index");

        }

    }

}
