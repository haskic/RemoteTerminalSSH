using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Newtonsoft.Json.Linq;
using ZModels;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace FrontEnd.Controllers
{
    public class UserController : Controller
    {
        private UserDbContext db;
        private readonly UserManager<IdentityUser> _userManager;
        private ILogger Logger;
        IWebHostEnvironment _appEnvironment;
        public UserController(UserDbContext context, UserManager<IdentityUser> userManager, ILogger<Program> logger,
            IWebHostEnvironment appEnvironment)
        {

            db = context;
            this._userManager = userManager;
            this.Logger = logger;
            this._appEnvironment = appEnvironment;
        }
        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            //ViewBag.username = HttpContext.Session.GetString("username");
            return View();
        }
        [Authorize]
        [Route("/user/terminal/{userId}/{name}")]
        public ActionResult Terminal(string userId, string name)
        {
            TerminalMessage message = new TerminalMessage();
            var result = db.Desktops.Where(v => v.UserId == userId).Where(v=>v.Name == name).FirstOrDefault();
            if (result == null)
            {
                message.Error = new TerminalErrorModel()
                {
                    ErrorText = $" You can't get access to Terminal : {name}",
                    ErrorType = "Access denied.",
                    UserId = userId,
                    TerminalName = name
                };

                return View(message);
            }

            message.Success = new TerminalSuccessModel()
            {
                TerminalName = name,
                TerminalIp = result.Ip,
                SuccessText = $"Connection to Terminal :: {name} with ip :: {result.Ip} - SUCCESS"

        };

            return View(message);
        }
        [Authorize]
        [Route("/user/{userId}")]
        public ActionResult CurrentUser()
        {

            return View("GuestProfile");

        }


        [Authorize]
        [HttpPost]
        [Route("user/terminal/add")]
        public async Task<JsonResult> AddDesktop([FromBody] Desktop desktop)
        {
            string result;
            Desktop d = desktop;
            d.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            db.Add(d);
            if (await db.SaveChangesAsync() > 0) {
                result = "{\"result\":\"true\"}";
            }
            else {
                result = "{\"result\":\"false\"}";

            }

            return Json(result);
        }

        [Authorize]
        [HttpPost]
        [Route("user/terminal/share")]
        public async Task<JsonResult> ShareDesktop([FromBody]SharedTerminalPackage package)
        {
            string myId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Logger.LogError("OBJECT LENGTH " + package.GuestId);

            Logger.LogError("OBJECT");
            Logger.LogError(JsonSerializer.Serialize(package.Terminals));
            

            if (package.Terminals.Count > 0)
            {
                package.Terminals.ForEach(value =>
                {
                    var result = db.Desktops.Where(v => v.UserId == myId).Where(o => o.Name == value.Name).FirstOrDefault();
                    Logger.LogWarning("RESULT" + JsonSerializer.Serialize(result));
                    if (result != null) {
                        if (value.Access)
                        {
                            result.Shared = package.GuestId;
                            db.SaveChanges();
                            
                        }
                        else
                        {
                            if (result.Shared == package.GuestId)
                            {
                                result.Shared = null;
                                db.SaveChanges();

                            }
                        }
                    }
                    
                });
            }

            //Desktop d = desktop;
            //d.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            //db.Add(d);
            //if (await db.SaveChangesAsync() > 0)
            //{
            //    result = "{\"result\":\"true\"}";
            //}
            //else
            //{
            //    result = "{\"result\":\"false\"}";

            //}
            string response = "{\"result\":\"true\"}";

            return Json(response);
        }

        [HttpGet]
        [Route("user/terminal/get")]
        public JsonResult GetDesktops()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Logger.LogWarning("UserID  = " + userId);
            return Json(JsonSerializer.Serialize(db.Desktops.Where(p => p.UserId == userId).ToList()));
        }

        [HttpGet]
        [Route("user/terminal/getshared")]
        public JsonResult GetSharedDesktops()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Logger.LogWarning("UserID  = " + userId);
            return Json(JsonSerializer.Serialize(db.Desktops.Where(p => p.Shared == userId).ToList()));
        }

        [HttpGet]
        [Route("user/terminal/get/{userId}")]
        public JsonResult GetDesktopsByUserId(string userId)
        {
            string email = this.User.Identity.Name;
            Logger.LogWarning("EMAIL = " + email);
            return Json(JsonSerializer.Serialize(db.Desktops.Where(p => p.UserId == userId).ToList()));
        }

        [HttpGet]
        [Route("user/getname")]
        public JsonResult GetUserName()
        {
            var us = new {
                name = User.Identity.Name
            };
            return Json(us);
        }

        [HttpGet]
        [Route("user/getId")]
        public JsonResult GetUserId()
        {
            var obj = new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
        };
            return Json(obj);
        }


        [Authorize]
        [HttpPost]
        [Route("user/terminal/update")]
        public async Task<JsonResult> SetDesktop([FromBody] DesktopWithOldName dswon)
        {

            string result;
            //JObject jObject = JObject.Parse(data);
            //string name = data.oldname;

            Logger.LogError(dswon.OldName);
            //Desktop desktop = data["DesktopObj"].ToObject<Desktop>();

            //string OldName = "Terminal-3";
            //Logger.LogWarning("OLDNAME:" + OldName);
            //Logger.LogWarning("Name:" + desktop.Name);
            //Logger.LogWarning("userName:" + desktop.UserName);
            //Logger.LogWarning("Password:" + desktop.Password);

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var d = db.Desktops.Where(value => value.UserId == userId).Where(value => value.Name == dswon.OldName).FirstOrDefault();
            d.Name = dswon.DesktopObj.Name;
            d.UserName = dswon.DesktopObj.UserName;
            d.Ip = dswon.DesktopObj.Ip;
            d.Sudo = dswon.DesktopObj.Sudo;
            d.Password = dswon.DesktopObj.Password;
            if (await db.SaveChangesAsync() > 0)
            {
                result = "{\"result\":\"true\"}";
            }
            else
            {
                result = "{\"result\":\"false\"}";

            }
            return Json(result);
        }

        [Route("user/profile")]
        public ActionResult Profile()
        {

            return View("Profile");
        }

        [Route("user/profile/get")]
        public async Task<JsonResult> getUserInfoAsync() {
            var user = await this._userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Json(JsonSerializer.Serialize(db.UserInfos.Where(p => p.Email == user.Email).FirstOrDefault()));
        }

        [Route("user/profile/get/{userId}")]
        public async Task<JsonResult> GetUserInfoByIdAsync(string userId)
        {
            return Json(JsonSerializer.Serialize(db.UserInfos.Where(p => p.UserId == userId).FirstOrDefault()));
        }

        [HttpPost]
        [Route("user/profile/set")]
        public async Task<JsonResult> updateUserInfoAsync([FromBody] UserInfo updatedUser)
        {
            var user = await this._userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userInfo = db.UserInfos.Where(value => value.UserId == user.Id).FirstOrDefault();
            userInfo.LastName = updatedUser.LastName;
            userInfo.Name = updatedUser.Name;
            userInfo.Email = updatedUser.Email;
            userInfo.NickName = updatedUser.NickName;
            await db.SaveChangesAsync();
            return Json("{\"result\":\"true\"}");
        }
        [Route("user/profile/photoUpload")]
        [HttpPost]
        public async Task<IActionResult> onPhotoUpload(IList<IFormFile> files) {
            Logger.LogError("FILE UPLOADING");
            
            Logger.LogError("URL =" + this._appEnvironment.WebRootPath);

            foreach (IFormFile source in files)
            {
                //string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                string filename = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString() + ".png";
                filename = this.EnsureCorrectFilename(filename);

                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    await source.CopyToAsync(output);
            }


            Logger.LogError("FILE UPLOADING OK");

            return Ok();
            
        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
        private string GetPathAndFilename(string filename)
        {
            return this._appEnvironment.WebRootPath + "\\static\\" + filename;
        }



    }
}