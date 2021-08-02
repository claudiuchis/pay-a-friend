using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Pay.Identity.Registration.Commands.V1;

namespace Pay.Identity.Registration
{
    public class RegistrationController : Controller
    {
        RegistrationService _service;
        public RegistrationController(RegistrationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var vm = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new RegisterUser( 
                    Guid.NewGuid().ToString(),
                    model.FullName,
                    model.Email,
                    model.Password
                );

                await _service.Handle(command, default);
                return Redirect(model.ReturnUrl);
            }
            return View(model);
        }
    }
}