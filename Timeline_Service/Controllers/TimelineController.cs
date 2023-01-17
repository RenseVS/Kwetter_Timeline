using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Timeline_Service.DTOs;
using Timeline_Service.Services;
using static MassTransit.ValidationResultExtensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Timeline_Service.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimelineController : Controller
    {
        private readonly TimelineService _timelineService;
        private readonly ILogger<TimelineController> _logger;
        public TimelineController(TimelineService timelineService, ILogger<TimelineController> logger)
        {
            _timelineService = timelineService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TweetDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TweetDTO>>> GetTimeline()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                var result = await _timelineService.GetTimeline(userId);
                sw.Stop();

                Log.Information("Succes: {Succes} in Controller: {Controller} Action: {Action} with Id: {Id} at DateTime: {DateTime} in Duration: {Duration}",
                new object[] { true, nameof(TimelineController), nameof(GetTimeline), userId, DateTime.Now.ToString(), sw.ElapsedMilliseconds, result });
                return Ok(result);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log.Information("Succes: {Succes} in Controller: {Controller} Action: {Action} with Id: {Id} at DateTime: {DateTime} in Duration: {Duration} with Exception: {Exception}",
                new object[] { false, nameof(TimelineController), nameof(GetTimeline), userId, DateTime.Now.ToString(), sw.ElapsedMilliseconds, ex.Message });
                return BadRequest(ex);
            }
        }

        //[Authorize("moderator")]
        [Route("test/moderator/{userid}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateTimeline(string userid)
        {
            string userId = userid;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                var result = await _timelineService.GetTimeline(userId);
                sw.Stop();

                Log.Information("Succes: {Succes} in Controller: {Controller} Action: {Action} with Id: {Id} at DateTime: {DateTime} in Duration: {Duration}",
                    new object[] { true, nameof(TimelineController), nameof(GetTimeline), userId, DateTime.Now.ToString(), sw.ElapsedMilliseconds});
                return Ok(result);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log.Information("Succes: {Succes} in Controller: {Controller} Action: {Action} with Id: {Id} at DateTime: {DateTime} in Duration: {Duration} with Exception: {Exception}",
                new object[] { false, nameof(TimelineController), nameof(GetTimeline), userId, DateTime.Now.ToString(), sw.ElapsedMilliseconds, ex.Message});
                return BadRequest(ex);
            }
        }
    }
}

