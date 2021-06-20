using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static App.Identity.Registration.Commands.V1;

namespace App.Identity.Registration
{
    public class RegistrationController : Controller
    {
        RegistrationService _service;
        public RegistrationController(RegistrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterUser 
                {
                    UserId = Guid.NewGuid().ToString(),
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password
                };

                await _service.Handle(command, default);
                return Ok();
            }
            return View(model);
        }
    }
}