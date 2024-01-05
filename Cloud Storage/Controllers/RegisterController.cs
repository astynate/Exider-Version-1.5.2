using Cloud_Storage.Classes;
using Cloud_Storage.Models;
using Cloud_Storage.wwwroot.cs_classes;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace Cloud_Storage.Controllers
{
    public class RegisterController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {

            // Post ViewBag Data About Controller Type
            ViewBag.Name = "SingUp";

            // Return View (Register)
            return View();

        }

        [HttpPost]
        public IActionResult Index(string name, string surname, string nickname, string email, string password)
        {

            // Use Validation Function
            if (Verifier.VerifyRegister(name, surname, nickname, email, password))
            {

                // Create Data Base
                using (var data_base = new UserContext())
                {

                    // Create UserModel in UserContext
                    var user_model = new User
                    {

                        Name = name,
                        Surname = surname,
                        Email = email,
                        Nickname = nickname,
                        Password = Hash.Encode(password),
                        Root = Database.GenerateUniqueRootID(),
                        StorageSpace = 1024

                    };

                    // Add UserModel to DataBase
                    data_base.Users.Add(user_model);

                    // Save DataBase Changes
                    data_base.SaveChanges();

                    // Copy Avatar
                    if (System.IO.File.Exists(Options.Path + "/0.png"))
                    {

                        System.IO.File.Copy(Options.Path + "/0.png", Options._userAvatarPath + user_model.Root);

                    }

                    // Copy Header
                    if (System.IO.File.Exists(Options.Path + "/default_header.png"))
                    {

                        System.IO.File.Copy(Options.Path + "/default_header.png", Options._userHeaderPath + user_model.Root);

                    }

                }

                string confirm_code = "";

                for (int i = 0; i < 6; i++)
                {

                    Random rand = new Random();

                    int rand_int = rand.Next(13);
                    char[] letters = { 'F', 'G', 'E', 'T' };

                    if (rand_int > 9)
                    {

                        confirm_code += letters[rand_int - 9];

                    }

                    else
                    {

                        confirm_code += rand_int;

                    }

                }

                TempData["data"] = confirm_code;

                EmailHandler.SendEmailConfirmation(email, name, confirm_code);

                return RedirectToAction("confirm", "register");

            }

            // Return View (Login)
            return RedirectToAction("index", "register");

        }

        [HttpGet]
        public IActionResult Confirm()
        {

            // Convert Temp Data as String
            var code = TempData["data"] as string;

            // Create Temp Data
            TempData["data"] = code;

            // Return View (Confirm)
            return View();

        }

        [HttpPost]
        public IActionResult Confirm(string code)
        {

            string? true_code = TempData["data"] as string;

            if (true_code != null && code != null && code.ToLower() == true_code.ToLower())
            {

                return RedirectToAction("index", "login");

            }

            // Create Temp Data
            TempData["data"] = true_code;

            return RedirectToAction("confirm", "register");

        }

    }

}
