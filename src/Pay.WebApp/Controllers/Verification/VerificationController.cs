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
        public async Task Verify(VerificationInputModel model)
        {
            var customerId = User.Claims.Where( claim => claim.Type == "sub").FirstOrDefault().Value;
            await _service.CreateDraftVerificationDetails(new CreateDraftVerificationDetails {
                VerificationDetailsId = Guid.NewGuid().ToString(),
                CustomerId = customerId
            });
        }
    }
}