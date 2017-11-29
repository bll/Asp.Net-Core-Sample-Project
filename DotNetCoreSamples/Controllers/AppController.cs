using DotNetCoreSamples.Data;
using DotNetCoreSamples.Services;
using DotNetCoreSamples.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreSamples.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly MyDbContext _context;

        public AppController(IMailService mailService, MyDbContext context)
        {
            _mailService = mailService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }

        [HttpPost("contact")]
        public ActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // send email
                _mailService.SendMessage("bilal@bilal.com", model.Subject, model.Message);
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }

            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }

        public IActionResult Shop()
        {
            //var result = _context.Products
            //    .OrderBy(p => p.Category).ToList();

            var result = from p in _context.Products
                         orderby p.Category
                         select p;

            return View(result.ToList());
        }
    }
}
