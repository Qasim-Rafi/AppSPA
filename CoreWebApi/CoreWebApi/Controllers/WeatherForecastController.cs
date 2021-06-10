using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/webAPI/TestApp")]
        public string TestApp()
        {
            return "Your application is running fine. Checked at: " + DateTime.Now.ToString();
        }
        [HttpGet("/webAPI/TestAppByParam/{value}")]
        public string TestAppByParam(string value)
        {
            return "Your value is: " + value;
        }
        [HttpPost("/webAPI/TestAppByPost")]
        public IActionResult TestAppByPost([FromBody] ExpandoObject objectt)
        {
            dynamic sampleObject = objectt;
            return Ok(new
            {
                YourValue = "Your value is: " + sampleObject.value,
                Name = "ijaz",
                Designation = "Software Engineer"
            });
        }
        [HttpPost("/webAPI/TestAppByPostWithAuth"), Authorize]
        public IActionResult TestAppByPostWithAuth([FromBody] ExpandoObject objectt)
        {
            dynamic sampleObject = objectt;
            return Ok(new
            {
                YourValue = "Your value is: " + sampleObject.value,
                Name = "ijaz",
                Designation = "Software Engineer"
            });
        }
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.UtcNow.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
