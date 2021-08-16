using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using Pay.WebApp.Models;
using Pay.WebApp.Configs;

namespace Pay.WebApp.Pages.Start
{
    [AllowAnonymous]
    public class StartController : Controller
    {
        private readonly ILogger<StartController> _logger;
        private readonly string _signUpUrl;
        private readonly string _signInUrl;
        private readonly string _returnUrl;

        public StartController(
            IOptions<IdentityProviderConfiguration> identityConfig,
            IOptions<LocalUrls> localUrls,
            ILogger<StartController> logger
        )
        {
            _logger = logger;
            var config = identityConfig.Value;
            _signUpUrl = $"{config.Authority}{config.SignUp}";
            _signInUrl = $"{config.Authority}{config.SignIn}";
            _returnUrl = localUrls.Value.ReturnUrl;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var encodedReturnUrl = HttpUtility.UrlEncode(_returnUrl);
                var vm = new StartViewModel
                {
                    SignUpUrl = $"{_signUpUrl}?ReturnUrl={encodedReturnUrl}",
                };
                return View(vm);
            }
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
