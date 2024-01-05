using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cloud_Storage.Controllers
{

    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            // Post ViewBag Data About Controller Type
            ViewBag.Name = "SingIn";

            // Return View (Login)
            return View();

        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {

            // Create SQLite Connection
            using (var connection = new SqliteConnection($"Data Source=user_data.db"))
            {

                // Open SQLite Connection
                connection.Open();

                // Create SQL Request
                SqliteCommand command = new SqliteCommand("SELECT * FROM Users", connection);

                // Execute Request
                using (SqliteDataReader reader = command.ExecuteReader())
                {

                    // Check Rows
                    if (reader.HasRows)
                    {

                        // Open SqlReader 
                        while (reader.Read())
                        {

                            // Input Fields
                            var input_email = reader.GetValue(4);
                            var nickname = reader.GetValue(3);
                            var input_password = reader.GetValue(5);

                            // Check User Data
                            if (((string)input_email == email || (string)nickname == email) && (string)input_password == Hash.Encode(password))
                            {

                                // Create User Claims
                                var claims = new List<Claim> { 
                                    
                                    new Claim(ClaimTypes.Email, (string)reader.GetValue(4)),
                                    new Claim("sub", reader.GetValue(6).ToString())
                                
                                };

                                // Create JWT Object
                                var jwt = new JwtSecurityToken(

                                    issuer: Options.Issuer,
                                    audience: Options.Audience,
                                    claims: claims,
                                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                                    signingCredentials: new SigningCredentials(Options.GetSecurityKey(), SecurityAlgorithms.HmacSha256)

                                );

                                // Get Access token
                                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                                // Append JWT-Token Cookie
                                Response.Cookies.Append(".AspNetCore.Mvc.CookieTempDataProvider", token);

                                // Return View (Home)
                                return RedirectToAction("index", "home");

                            }

                        }
                    }

                }
            }

            // Return View (Index)
            return RedirectToAction("index", "login");

        }

        [HttpGet]
        public IActionResult GetConfirmation(string code)
        {

            // Convert Data as String
            var data = TempData["data"] as string;

            // Check Input Code
            if (data == code)
            {

                // Return View (Login)
                return RedirectToAction("Login");

            }

            // Return View (Confirm)
            return RedirectToAction("Confirm");

        }

    }
}
