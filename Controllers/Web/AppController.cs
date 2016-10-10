using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TheWorld.Controllers.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services;
    using ViewModels;

    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRepository repository,
            ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var data = _repository.GetAllTrips();

                return View(data);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index page: {ex.Message}");
                return Redirect("/error");
            }
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "We don't support AOL addresses.  Get out of here with that noise.");
            }

            if (ModelState.IsValid)
            {
                
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "Contact", model.Message);
                ModelState.Clear();

                ViewBag.UserMessage = "Message Sent!";
            }

            return View();
        }

        [Authorize]
        public IActionResult Trips()
        {
            try
            {
                var trips = _repository.GetAllTrips();
                return View(trips);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index page: {ex.Message}");
                return Redirect("/error");
            }
        }

        public IActionResult About()
        {
            return View();
        }

    }
}
