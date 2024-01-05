using Cloud_Storage.Classes;
using Cloud_Storage.Classes.Adapters;
using Cloud_Storage.Classes.Data;
using Cloud_Storage.Classes.Handlers;
using Cloud_Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Policy;

namespace Cloud_Storage.Controllers
{
    public class FilesController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Index(string? id)
        {

            // Get JWT-Token Form Cookie
            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];

            // Get Email From JWT-Token
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            user.Header = null;
            user.Avatar = Compression
                .ResizeImageWidth(user.Avatar, 100);

            ViewBag.currentUser = user;

            ViewBag.ActiveButton = "files";
            ViewBag.LeftPanelData = Database.GetLeftPanelDataByEmail(Email);
            ViewBag.OccupiedSpace = Convert.ToInt32(Storage.GetUserOccupiedSpace(Email) / user.StorageSpace * 100);

            // Validate Id (NOT NULL)
            if (id == null || id == "")
            {

                // Set Root Id By Email
                id = Database.GetUserByEmail(Email).Root;

            }

            // Send Current Folder Id
            ViewData["current_folder"] = id;

            // Create Files Variable
            FileView[] files = Database.GetFilesByFolderId(id);

            // Create Folders Variable
            FolderView[] folders = Database.GetFoldersByFolderId(id);

            // Create Files Model Variable
            FilesModel filesModel = new FilesModel
            {
                Files = files,
                Folders = folders,
                Path = Storage.GetCurrentPathByFolderId(id)
            };

            string userAgent = Request.Headers["User-Agent"].ToString();
            bool isMobile = userAgent.Contains("Mobi");

            if (isMobile)
            {
                return View("IndexMobile", filesModel);
            }
            else
            {
                return View(filesModel);
            }

        }

        [HttpGet]
        [Authorize]
        public IActionResult File(string? id)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            FileContext db = new FileContext();
            FileModel? file = db.Files.FirstOrDefault(x => x.id == id);

            if (file != null) 
            {

                return Ok(new FileView(user, file));
            
            }

            return BadRequest();

        }

        [HttpPost]
        [Authorize]
        public void Save(string? id, string? text)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            FileContext db = new FileContext();
            FileModel? file = db.Files.FirstOrDefault(x => x.id == id);

            if (file != null)
            {

                FileView fileView = new FileView(user, file);
                StreamWriter writer = new StreamWriter(Options.Path + id);

                string resultText = MessageAdapter.
                    ConvertNewlineFromHtmlToCSharp((text == null) ? "" : text);

                writer.WriteLine(resultText);
                writer.Close();

            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateFile(string? id, string? folder_name, string? type)
        {

            if (string.IsNullOrEmpty(folder_name) || string.IsNullOrEmpty(type))
                return BadRequest();

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            var user = Database.GetUserByEmail(Email);

            if (Convert.ToInt32(1 / Math.Pow(1024, 2)) > user.StorageSpace - Convert.ToInt32(Storage.GetUserOccupiedSpace(Email)))
                return RedirectToAction("index", new { id = id, error = "not_enough_space" });

            if (string.IsNullOrEmpty(id) || id == "null")
                id = Database.GetUserByEmail(Email).Root;

            string FileId = Database
                .GenerateUniqueFileID();

            using (var data_base = new FileContext())
            {

                FileModel file_model = new FileModel
                (
                    user,
                    FileId,
                    folder_name,
                    type,
                    id
                );

                data_base.Files.Add(file_model);
                data_base.SaveChanges();

            }

            using (System.IO.File.Create(Options.Path + "/" + FileId)) { };

            return Ok();

        }

        [HttpPost]
        [Authorize]
        public IActionResult Rename(string id, string name)
        {

            // Get JWT-Token Form Cookie
            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];

            // Get Email From JWT-Token
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            using (FileContext data_base = new FileContext())
            {

                FileModel? file = data_base.Files.FirstOrDefault(x => x.id == id);

                if (file != null && AccessHandler.CheckFileAccess(Database.GetUserByEmail(Email), file.id))
                {

                    file.name = (name != null) ? name : "None";
                    data_base.Update(file);
                    data_base.SaveChanges();

                    return Ok();

                }

                else
                {

                    using (var db = new FolderContext())
                    {

                        Folder? folder = db.Folders.FirstOrDefault(x => x.id == id);

                        if (folder != null && AccessHandler.CheckFolderAccess(Database.GetUserByEmail(Email), folder.id))
                        {

                            folder.name = (name != null) ? name : "None";
                            db.Update(folder);
                            db.SaveChanges();

                            return Ok();

                        }

                    }

                }

            };

            // Return View (Index)
            return StatusCode(500);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateFolder(string? id, string folder_name)
        {

            // Get JWT-Token Form Cookie
            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];

            // Get Email From JWT-Token
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            // Validate Id (NOT NULL)
            if (string.IsNullOrEmpty(id) || id == "null")
            {

                // Set Root Id By Email
                id = Database.GetUserByEmail(Email).Root;

            }

            using (var data_base = new FolderContext())
            {

                Folder folder = new Folder(
                    Database.GetUserByEmail(Email), folder_name, id);

                try
                {

                    data_base.Add(folder);
                    data_base.SaveChanges();

                    return Ok();

                }

                catch (Exception ex)
                {

                    Console.WriteLine("Произошло исключение: " + ex.Message);

                }

            }

            // Return View (Index)
            return StatusCode(500);

        }

        [HttpPost]
        [Authorize]
        public IActionResult UploadFiles(string? id, IFormFile[] UploadFile)
        {

            // Get JWT-Token Form Cookie
            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];

            // Get Email From JWT-Token
            var Email = JwtHandler.GetEmailByToken(JwtToken);
            
            var user = Database.GetUserByEmail(Email);

            // Validate Id (NOT NULL)
            if (string.IsNullOrEmpty(id) || id == "null")
            {

                // Set Root Id By Email
                id = Database.GetUserByEmail(Email).Root;

            }

            long size = 0;

            if (UploadFile != null) 
            {

                foreach (var file in UploadFile)
                {

                    size += file.Length;

                }

                if (Convert.ToInt32(size / Math.Pow(1024, 2)) > user.StorageSpace - Convert.ToInt32(Storage.GetUserOccupiedSpace(Email)))
                {

                    return RedirectToAction("index", new { id = id, error = "not_enough_space" });

                }

            }

            // Validate File Form
            if (UploadFile != null && UploadFile.Length >= 1)
            {

                for (int i = 0; i < UploadFile.Length; i++)
                {

                    // Get File Name From Form 
                    string FileName = UploadFile[i].FileName;

                    // Generate Unique File Id
                    string FileId = Database.GenerateUniqueFileID();

                    // Get Path From Options
                    string Path = Options.Path + "/" + FileId;

                    // Create Data Base
                    using (var data_base = new FileContext())
                    {

                        // Create File Model
                        FileModel file_model = new FileModel
                        (
                            user,
                            FileId,
                            FileName.Split('.')[0],
                            FileName.Split('.')[1],
                            id
                        );

                        // Add UserModel to DataBase
                        data_base.Files.Add(file_model);

                        // Save DataBase Changes
                        data_base.SaveChanges();

                    }

                    using (var fileStream = new FileStream(Path, FileMode.Create))
                    {
                        UploadFile[i].CopyTo(fileStream);
                    }

                }

            }

            // Return View (Index)
            return RedirectToAction("index", "files", new { id = id });

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Download([FromBody] object inputJSON)
        {

            Dictionary<string, string>? dictionary = JsonConvert
                .DeserializeObject<Dictionary<string, string>>(inputJSON.ToString());

            using (var db = new FileContext())
            {

                if (dictionary != null && dictionary["id"] != null)
                {

                    FileModel? file = db.Files.FirstOrDefault(x => x.id == dictionary["id"]);

                    if (file != null)
                    {

                        try
                        {

                            Response.Headers.Add("File-Type", file.type);

                            return File(System.IO.File.ReadAllBytes($"{Options.Path}{file.id}"), 
                                "application/octet-stream", $"{file.name}.{file.type}");

                        }

                        catch (Exception ex)
                        {

                            Console.WriteLine(ex.Message);

                        }

                    }

                }
                
            }

            return StatusCode(500);

        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] object inputJSON)
        {

            var JwtToken = Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];
            var Email = JwtHandler.GetEmailByToken(JwtToken);

            User user = Database.GetUserByEmail(Email);

            Dictionary<object, object>? dictionary = JsonConvert
                .DeserializeObject<Dictionary<object, object>>(inputJSON.ToString());

            try
            {

                if (Convert.ToBoolean(dictionary["folder"]) == true)
                {

                    if (AccessHandler.CheckFolderAccess(user, dictionary["id"].ToString()))
                        Storage.DeleteFolderById(dictionary["id"].ToString());

                }

                else
                {

                    if (AccessHandler.CheckFileAccess(user, dictionary["id"].ToString()))
                        Storage.DeleteFileById(dictionary["id"].ToString());

                }

                return StatusCode(200);

            }

            catch (Exception ex) 
            {

                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);

            }

        }

    }

}
