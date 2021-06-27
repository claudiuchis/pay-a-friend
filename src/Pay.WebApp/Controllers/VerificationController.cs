using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Pay.WebApp
{
    [Authorize]
    public class VerificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }   
    }
}