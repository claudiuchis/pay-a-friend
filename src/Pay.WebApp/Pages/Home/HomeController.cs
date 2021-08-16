using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using Pay.WebApp.Models;
using Pay.WebApp.Configs;

namespace Pay.WebApp.Pages.Home
{
    public class HomeController : Controller
    {
        private readonly CustomersService _customersService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            CustomersService customersService
        )
        {
            _logger = logger;
            _customersService = customersService;
        }

        public async Task<IActionResult> Index()
        {
            var customerId = User.Claims.Where( claim => claim.Type == "sub").FirstOrDefault().Value;
            var customerModel = await _customersService.GetCustomerById(customerId);

            if (customerModel == null || customerModel.DetailsSubmitted == false)
            {
                return RedirectToAction("Verify", "Verification");
            }

            if (customerModel.DetailsVerified == false)
            {
                return RedirectToAction("Pending", "Verification");
            }

            return View();
        }
    }
}
