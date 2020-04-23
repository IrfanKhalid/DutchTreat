using DutchTreat.Data.Entities;
using DutchTreat.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController: Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> manager;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> manager )
        {
            this.logger = logger;
            this.manager = manager;
        }

        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "app");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginView)
        {
            if(ModelState.IsValid)
            {
                var Result= await manager.PasswordSignInAsync(loginView.UserName,loginView.Password,loginView.RememberMe, false);
                if(Result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query.Keys.First());
                    }
                    else
                    {
                        return RedirectToAction("Shop", "app");
                    }
                }
            }
            ModelState.AddModelError("", "Failed to Login");
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Logout()
        {
            await manager.SignOutAsync();
            return RedirectToAction("Index", "app");
        }


    }
}
 