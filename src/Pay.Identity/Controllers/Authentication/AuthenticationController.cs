using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using static Pay.Identity.Queries.ReadModels;
using Microsoft.AspNetCore.Authentication;

namespace Pay.Identity.Authentication
{
    public class AuthenticationController: Controller
    {
        AuthenticationService _service;
        public AuthenticationController(AuthenticationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel 
            {
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                (bool result, UserDetails userDetails) = await _service.CheckCredentials(model.Email, model.Password);

                if (result)
                {
                    var isuser = new IdentityServerUser(userDetails.UserId)
                    {
                        DisplayName = userDetails.FullName,
                        AdditionalClaims = new Claim[] {
                            new Claim(ClaimTypes.Email, userDetails.Email)
                        }
                    };
                    await HttpContext.SignInAsync(isuser, null);
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return View(model);
        }
    }
}