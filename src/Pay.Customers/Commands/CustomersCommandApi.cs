using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Pay.Verification.Commands.V1;

namespace Pay.Verification
{
    [ApiController]
    [Route("api/customer")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        CustomersCommandService _service;
        public CustomersController(CustomersCommandService service) => _service = service;

        [HttpPost]
        public Task CreateCustomer([FromBody] CreateCustomer command)
            => _service.Handle(command, default);    

        [HttpPost]
        [Route("dob")]
        public Task AddDateOfBirth([FromBody] AddDateOfBirth command)
            => _service.Handle(command, default);    

        [HttpPost]
        [Route("address")]
        public Task AddAddress([FromBody] AddAddress command)
            => _service.Handle(command, default);    

        [HttpPost]
        [Route("submit")]
        public Task Submit()
            => _service.Handle(
                new SubmitDetailsForVerification { CustomerId = User.Claims.FirstOrDefault (c => c.Type == ClaimTypes.NameIdentifier).Value }, 
                default
            );  

        [HttpPost]
        [Route("verify")]
        public Task VerifyDetails([FromBody] VerifyCustomerDetails command)
            => _service.Handle(command, default);

    }
}