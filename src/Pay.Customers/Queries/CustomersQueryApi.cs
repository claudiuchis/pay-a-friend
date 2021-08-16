using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Eventuous;

namespace Pay.Customers.Queries
{
    [ApiController]
    [Route("/api/customers")]
    public class CustomersQueryApi : ControllerBase
    {

        CustomersQueryService _service;
        readonly ILogger<CustomersQueryApi> _log;
        
        public CustomersQueryApi(
            CustomersQueryService service,
            ILoggerFactory loggerFactory
        )
        {
            _service = service;
            _log = loggerFactory.CreateLogger<CustomersQueryApi>();
        } 
        
        [HttpGet]
        [Route("{id:string}")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            try 
            {
                var customer = await _service.GetCustomerById(customerId);
                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch(Exception e)
            {
                _log.LogError($"{e.Message} - {e.StackTrace}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
