using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SSHZHAKAR;
using SSHZHAKAR.Controllers;
using ZModels;
namespace FrontEnd.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IStringLocalizer<AuthController> _localizer;
        private UserDbContext db;
        public ILogger<Program> Logger { get; }

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, ILogger<Program> logger,
            UserDbContext context, IStringLocalizer<AuthController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Logger = logger;
            db = context;
            _localizer = localizer;
        }
        [Route("/reg")]
        public IActionResult RegPage()
        {
            ViewData["Message"] = _localizer["Registration"];
            ViewBag.header = false;
            return View("Registration");
        }

        [AllowAnonymous]
        [Route("/login")]
        public IActionResult LoginPage()
        {
            ViewBag.header = false;
            return View("Login");
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginPage(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            Logger.LogDebug("LOGIN = " + loginModel.Email);
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                if (signInResult.Succeeded)
                {
                    HttpContext.Session.SetString("username", loginModel.Email);
                    ViewBag.username = loginModel.Email;
                    return RedirectToAction("Index", "User");
                }
            }
            return View("Login");
        }

        [HttpPost]
        [Route("/reg")]
        public async Task<IActionResult> RegPage(RegistrationModel regModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Registration");

            }
            var user = new IdentityUser
            {
                Email = regModel.Email,
                UserName = regModel.NickName
            };
            var result = await _userManager.CreateAsync(user, regModel.Password);

            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, regModel.Password, false, false);
                if (signInResult.Succeeded)
                {
                    UserInfo userInfo = new UserInfo()
                    {
                        Name = regModel.Name,
                        LastName = regModel.LastName,
                        NickName = regModel.NickName,
                        Email = regModel.Email,
                        UserId = user.Id
                    };
                    db.UserInfos.Add(userInfo);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "User");
                }

            }
            return View("Registration");
        }
        [Route("/logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}