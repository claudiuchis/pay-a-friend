using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Pay.WebApp.Commands;
using System.Security.Claims;

namespace Pay.WebApp
{
    [Authorize]
    public class VerificationController : Controller
    {
        VerificationService _service; 
        public VerificationController(VerificationService service) => _service = service;

        [HttpGet]
        public IActionResult Verify()
        {
            return View();
        }   

        [HttpPost]
        public async Task<IActionResult> Verify(VerificationModel model)
        {
            if (ModelState.IsValid)   
            {
                await _service.SendVerificationDetails(model);
                return Redirect("/Home");
            }
            return View(model);
        }
    }
}