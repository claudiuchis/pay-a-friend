using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Pay.Verification.Commands.V1;

namespace Pay.Verification
{
    [ApiController]
    [Route("api/verification")]
    [Authorize]
    public class VerificationController : ControllerBase
    {
        VerificationCommandService _service;
        public VerificationController(VerificationCommandService service) => _service = service;

        [HttpPost]
        [Route("submit")]
        public Task SubmitVerification([FromBody] SubmitVerification command)
            => _service.Handle(command, default);    

        [HttpPut]
        [Route("complete")]
        public Task CompleteVerification([FromBody] CompleteVerification command)
            => _service.Handle(command, default);

    }
}