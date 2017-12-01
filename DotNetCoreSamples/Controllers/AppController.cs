using DotNetCoreSamples.Data;
using DotNetCoreSamples.Services;
using DotNetCoreSamples.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCoreSamples.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        // private readonly MyDbContext _context;
        private readonly IRepository _repository;

        public AppController(IMailService mailService, MyDbContext context, IRepository repository)
        {
            _mailService = mailService;
            // _context = context;
            _repository = repository;
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

        [Authorize]
        public IActionResult Shop()
        {
            //var result = _context.Products
            //    .OrderBy(p => p.Category).ToList();

            //var result = from p in _context.Products
            //             orderby p.Category
            //             select p;

            var result = _repository.GetAllProduct();

            return View(result.ToList());
        }
    }
}
