using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QuartzCore.Blazor.Shared;
using QuartzCore.MongoDB.LogEntity;
using QuartzCore.MongoDB.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzCore.Blazor.Server.Controllers
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
        IMongoRepository<QzRunLogMoEntity> _mongoRepository;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMongoRepository<QzRunLogMoEntity> mongoRepository)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            QzRunLogMoEntity log = new QzRunLogMoEntity();
            log.TasksQzId = 2;
            log.AppId = "应用02";
            log.LogType = 0;
            log.LogText = "额是测试";
            log.LogTime = DateTime.Now.AddHours(8); //默认用UTC 需要中国是东八区要加8小时
            log.Milliseconds = "100毫秒";
            var res = _mongoRepository.InsertAsync(log).Result;
            var result = _mongoRepository.GetListAsync().Result;
            var builder = Builders<QzRunLogMoEntity>.Filter;
            var filter = builder.Where(d => d.LogTime > DateTime.Now.AddHours(-2) && d.LogTime < DateTime.Now);
            var result2 = _mongoRepository.GetAsync(filter).Result;




            //throw new Exception("测试报错");
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
