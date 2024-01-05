using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Cloud_Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Storage.Controllers
{
    public class ProfileController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);
            UserModel user = Database.GetUserByEmail(Email);

            ViewBag.ActiveButton = "profile";
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            user.Header = Compression
                .ResizeImageWidth(user.Header, 1700);

            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 200);

            ViewBag.currentUser = user;

            return View();

        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(IFormFile avatar, IFormFile header, string name, string surname, string nickname)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            UserModel user = Database.GetUserByEmail(Email);

            if (avatar != null)
            {

                using (var fileStream = new FileStream(Options._userAvatarPath + user.Root, FileMode.Create))
                {
                    avatar.CopyTo(fileStream);
                }

            }

            if (header != null)
            {

                using (var fileStream = new FileStream(Options._userHeaderPath + user.Root, FileMode.Create))
                {
                    header.CopyTo(fileStream);
                }

            }

            using (var data_base = new UserContext())
            {

                var input_user = data_base.Users.FirstOrDefault(x => x.Id == user.Id);

                if (input_user != null)
                {

                    if (name != null)
                        input_user.Name = name;
                    if (surname != null)
                        input_user.Surname = surname;
                    if (nickname != null)
                        input_user.Nickname = nickname;

                    data_base.SaveChanges();

                }

            }

            return RedirectToAction("index", "profile");

        }

    }

}
