using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository dutchContext;

        public AppController(IMailService mailService,IDutchRepository dutchContext)
        {
            this.mailService = mailService;
            this.dutchContext = dutchContext;
        }
        public IActionResult Index()
        {
            var data = dutchContext.GetProduct();
            return View();
        }

        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact";
            //throw new InvalidOperationException("Bad Things Happen");

            return View();
        }

        [HttpPost("Contact")]
        public IActionResult Contact(ContactViewModel data)
        {
            ViewBag.Title = "Contact";
            if (ModelState.IsValid)
            {
                mailService.sendEmail(data.Email,data.Subject,data.MessageBody);
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            else
            {
                //Do not send email
            }
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About";
            return View();
        }
        [Authorize]
        public IActionResult Shop()
        {
            var Results = dutchContext.GetProduct();
            return View(Results.ToList());
        }
    }
}