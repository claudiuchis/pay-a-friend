using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Pay.Verification.Commands.V1;

namespace Pay.Verification
{
    [ApiController]
    [Route("api/verify")]
    [Authorize]
    public class VerificationController : ControllerBase
    {
        VerificationService _service;
        public VerificationController(VerificationService service) => _service = service;

        [HttpPost]
        [Route("draft")]
        public Task CreateDraft([FromBody] CreateVerificationDetailsDraft command)
            => _service.Handle(command, default);    
    }
}