using ContactApp.Application.Dto;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Web.Controllers
{
    public class ContactController : Controller
    {
        private IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;   
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
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
