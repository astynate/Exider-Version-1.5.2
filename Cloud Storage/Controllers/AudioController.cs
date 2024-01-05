using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Storage.Controllers
{
    public class AudioController : Controller
    {

        [Authorize]
        public IActionResult Index()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            ViewBag.ActiveButton = "audio";
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            return View();

        }

    }

}
