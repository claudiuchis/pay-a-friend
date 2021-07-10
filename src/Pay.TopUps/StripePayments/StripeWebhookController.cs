using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Pay.TopUps.StripePayments
{
    [ApiController]
    [Route("api/topup/stripe")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly StripeWebhookService _service;

        public StripeWebhookController(StripeWebhookService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                await _service.React(stripeEvent);
            }
            catch (StripeException e)
            {
                return BadRequest(e);
            }
            return Ok();
        }
    }
}
