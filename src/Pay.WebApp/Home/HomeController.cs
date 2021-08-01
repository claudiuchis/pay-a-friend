using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using Pay.WebApp.Models;
using Pay.WebApp.Configs;

namespace Pay.WebApp.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _signUpUrl;
        private readonly string _signInUrl;

        public HomeController(
            IOptions<IdentityProviderConfiguration> identityConfig,
            ILogger<HomeController> logger
        )
        {
            _logger = logger;
            var config = identityConfig.Value;
            _signUpUrl = $"{config.Authority}{config.SignUp}";
            _signInUrl = $"{config.Authority}{config.SignIn}";
        }

        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                SignUpUrl = _signUpUrl,
                SignInUrl = _signInUrl                   
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home");
        }
    }
}
