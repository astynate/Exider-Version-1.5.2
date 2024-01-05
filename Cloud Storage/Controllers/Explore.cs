using Cloud_Storage.Classes;
using Cloud_Storage.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Storage.Controllers
{
    public class Explore : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            UserModel user = Database.GetUserByEmail(Email);

            return View(user);

        }
    }
}
