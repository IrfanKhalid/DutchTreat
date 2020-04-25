using DutchTreat.Data.Entities;
using DutchTreat.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController: Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly SignInManager<StoreUser> manager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration config;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> manager, UserManager<StoreUser> userManager, IConfiguration config)
        {
            this.logger = logger;
            this.manager = manager;
            this.userManager = userManager;
            this.config = config;
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

        [HttpPost]

        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel loginView ) 
        {
        
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginView.UserName);
                var result = await manager.CheckPasswordSignInAsync(user, loginView.Password, false);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString())
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
                    var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        config["Tokens:Issuer"],
                        config["Tokens:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(25),
                        signingCredentials: credentails
                        );
                    var Result = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    return Created("", Result);
                    }
            
            }
            return BadRequest("Not Authenticated");
        }


    }
}
 