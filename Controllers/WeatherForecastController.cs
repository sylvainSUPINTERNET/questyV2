using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRChat.Hubs;
using SignalRChat.IHubsMessage;
using StackExchange.Redis;

namespace EasyTransfer.Controllers
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
        private readonly IConnectionMultiplexer _redis;

        private readonly IHubContext<ChatHub> _hubContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubContext<ChatHub> hub, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _hubContext = hub;
            _redis=redis;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            var db = this._redis.GetDatabase();
            var pong = db.Ping();
            Console.WriteLine(pong);


            // ChatMessage msg = new ChatMessage();
            // msg.User = "sylvain";
            // msg.Message = "BONJOUR";
            // this._hubContext.Clients.All.Spam(msg);
            

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
