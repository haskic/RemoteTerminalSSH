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
using Microsoft.AspNetCore.Localization;

namespace FrontEnd.Controllers
{
    public class UserController : Controller
    {
        private UserDbContext db;
        private readonly UserManager<IdentityUser> _userManager;
        private ILogger Logger;
        private readonly IWebHostEnvironment _appEnvironment;
        public UserController(UserDbContext context, UserManager<IdentityUser> userManager, ILogger<Program> logger,
            IWebHostEnvironment appEnvironment)
        {

            db = context;
            this._userManager = userManager;
            this.Logger = logger;
            this._appEnvironment = appEnvironment;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        [Route("/user/terminal/{userId}/{name}")]
        public ActionResult Terminal(string userId, string name)
        {
            TerminalMessage message = new TerminalMessage();
            var result = db.Desktops.Where(v => v.UserId == userId).Where(v => v.Name == name).FirstOrDefault();
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
        public async Task<JsonResult> AddDesktop([FromBody] Terminal terminal)
        {
            string result;
            Terminal d = terminal;
            d.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            db.Add(d);
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

        [Authorize]
        [HttpPost]
        [Route("user/terminal/share")]
        public JsonResult ShareDesktop([FromBody]SharedTerminalPackage package)
        {
            string myId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (package.Terminals.Count > 0)
            {
                package.Terminals.ForEach(value =>
                {
                    var result = db.Desktops.Where(v => v.UserId == myId).Where(o => o.Name == value.Name).FirstOrDefault();
                    Logger.LogWarning("RESULT" + JsonSerializer.Serialize(result));
                    if (result != null)
                    {
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
            var us = new
            {
                name = User.Identity.Name,
                userId = User.FindFirst(ClaimTypes.NameIdentifier).Value
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
            Logger.LogError("USER ID IN METHOD" + User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Logger.LogError("GET ID METHOD");
            return Json(obj);
        }

        [Authorize]
        [HttpPost]
        [Route("user/terminal/update")]
        public async Task<JsonResult> SetDesktop([FromBody] EditedTerminal dswon)
        {

            string result;
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var d = db.Desktops.Where(value => value.UserId == userId).Where(value => value.Name == dswon.OldName).FirstOrDefault();
            d.Name = dswon.TerminalObj.Name;
            d.UserName = dswon.TerminalObj.UserName;
            d.Ip = dswon.TerminalObj.Ip;
            d.Sudo = dswon.TerminalObj.Sudo;
            d.Password = dswon.TerminalObj.Password;
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
        public async Task<JsonResult> GetUserInfoAsync()
        {
            var user = await this._userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Json(JsonSerializer.Serialize(db.UserInfos.Where(p => p.Email == user.Email).FirstOrDefault()));
        }

        [Route("user/profile/get/{userId}")]
        public JsonResult GetUserInfoById(string userId)
        {
            return Json(JsonSerializer.Serialize(db.UserInfos.Where(p => p.UserId == userId).FirstOrDefault()));
        }

        [HttpPost]
        [Route("user/terminal/unick")]
        public JsonResult CheckTerminalNameUnick([FromBody] string terminalName)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var d = db.Desktops.Where(p => p.UserId == UserId).Where(p => p.Name == terminalName).FirstOrDefault();
            if (d == null)
            {
                return Json("{\"result\":\"true\"}");
            }
            else
            {
                return Json("{\"result\":\"false\"}");
            }

        }

        [HttpPost]
        [Route("user/profile/set")]
        public async Task<JsonResult> UpdateUserInfoAsync([FromBody] UserInfo updatedUser)
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
        public async Task<IActionResult> OnPhotoUpload(IList<IFormFile> files)
        {
            foreach (IFormFile source in files)
            {
                string filename = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString() + ".png";
                filename = this.EnsureCorrectFilename(filename);
                using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                    await source.CopyToAsync(output);
            }
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
            return @"C:\Users\Alexander\source\repos\SSHZHAKAR\SSHZHAKAR\wwwroot\static\" + filename;
        }
    }
}