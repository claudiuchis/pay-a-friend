using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Pay.TopUps.Commands.Commands;

namespace Pay.TopUps.Commands
{
    [ApiController]
    [Route("[api/topup]")]
    public class TopUpsController : ControllerBase
    {
        private readonly ILogger<TopUpsController> _logger;
        private readonly TopUpsService _service;

        public TopUpsController(
            ILogger<TopUpsController> logger,
            TopUpsService service
            )
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public Task SubmitTopUp([FromBody] V1.SubmitTopUp command)
            => _service.Handle(command, default);
    }
}
