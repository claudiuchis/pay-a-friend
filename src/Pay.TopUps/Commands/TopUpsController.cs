using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Pay.TopUps.Commands.Commands;

namespace App.TopUps.Commands
{
    [ApiController]
    [Route("[api/topups]")]
    public class TopUpsController : ControllerBase
    {
        private readonly ILogger<TopUpsController> _logger;

        public TopUpsController(ILogger<TopUpsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Task SubmitTopUp([FromBody] SubmitTopUp)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
