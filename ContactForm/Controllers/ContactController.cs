using ContactApp.Application.Dto;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactService _contactService;
        private readonly ILoggerService _logger;
        public ContactController(IContactService contactService, ILoggerService loggerService)
        {
            _contactService = contactService;
            _logger = loggerService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ContactViewModel
            {
                Name = string.Empty,
                LastName = string.Empty,
                Email = string.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactViewModel model)
        {
            _logger.LogInformation("Received request for creating contact");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            _logger.LogInformation("Processing details for: {Name} {LastName}, Email: {Email}, IP: {IPAddress}",
            model.Name, model.LastName, model.Email, ipAddress);
            var contactDto = new ContactDto
            {
                Name = model.Name,
                LastName = model.LastName,
                Email = model.Email
            };
            var (success, message) = await _contactService.ProcessContactSubmissionAsync(contactDto, ipAddress);
            if (success) 
                return RedirectToAction(nameof(Success));
                ModelState.AddModelError("", message);
                return View(model);
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
