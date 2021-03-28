using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka;
using Volue.Job.Messages;
using Volue.Job.Persistance.DbContexts;
using Volue.Job.Persistance.Entities;
using Volue.Job.WebApi.Akka.Actors;
using Volue.Job.WebApi.Models;

namespace Volue.Job.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class DataPointsController : ControllerBase
    {
        private readonly AkkaConfig _akkaConfig;
        private readonly IDataPointDbContext _dbContext;
        public DataPointsController(AkkaConfig akkaConfig, IDataPointDbContext dbContext)
        {
            _akkaConfig = akkaConfig ?? throw new ArgumentNullException(nameof(akkaConfig));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost]
        public async Task<IActionResult> AddDataPoints([FromBody] IEnumerable<Models.DataPoint> data)
        {
            await _dbContext.DataPoints.AddRangeAsync(data.Select(item => new Persistance.Entities.DataPoint
            {
                Value = item.Value,
                Name = item.Name,
                Epoch = item.Epoch
            }));
            await _dbContext.SaveChangesAsync();
            return Created(String.Empty, data);
        }

        [HttpGet]
        public async Task<ActionResult<CalculationResult>> Calculate([FromQuery] string name,
            [FromQuery] long? from, [FromQuery] long? to)
        {
            var response = await AkkaSystemKeeper.ReaderActor.Ask(
                new CalculateDataPoint { DataPointName = name, From = from, To = to },
                TimeSpan.FromSeconds(_akkaConfig.ResponseTimeout));
            return GetResponse(response);
        }


        private ActionResult GetResponse(object response)
            => response.Match<ActionResult>()
                .With<Complete.Success>(success => Ok(success.Result))
                .With<Complete.Failure>(failure => BadRequest(failure.Reason))
                .ResultOrDefault(_ => throw new InvalidOperationException("Something went wrong."));
    }
}
