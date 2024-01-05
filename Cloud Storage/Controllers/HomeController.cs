using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Data;
using Cloud_Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Storage.Controllers
{
    public class homeController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            UserModel user = Database.GetUserByEmail(Email);

            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 200);

            ViewBag.currentUser = user;

            try
            {

                ViewBag.ActiveButton = "home";
                ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
                ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            } catch { }

            string userAgent = Request.Headers["User-Agent"].ToString();
            bool isMobile = userAgent.Contains("Mobi");

            string folderPath = Options.Path + "/__home__";
            string[] files = Directory.GetFiles(folderPath);

            List<byte[]> fileBytesList = new List<byte[]>();

            foreach (string filePath in files)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                fileBytesList.Add(fileBytes);
            }

            byte[][] filesByteArray = fileBytesList.ToArray();

            ViewBag.files = filesByteArray;

            if (isMobile)
            {
                return View("IndexMobile", user);
            }
            else
            {
                return View(user);
            }

        }

    }

}