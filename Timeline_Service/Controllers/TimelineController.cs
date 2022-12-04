using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Timeline_Service.DTOs;
using Timeline_Service.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Timeline_Service.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimelineController : Controller
    {
        private readonly TimelineService _timelineService;
        public TimelineController(TimelineService timelineService)
        {
            _timelineService = timelineService;
        }

        [Route("{UserID}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TweetDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TweetDTO>>> GetTimeline([FromRoute] string UserID)
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                if (UserID != userId) {
                    return Unauthorized();
                }
                var result = await _timelineService.GetTimeline(UserID);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("test/{UserID}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TweetDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TweetDTO>>> UpdateTimeline([FromRoute] string UserID)
        {
            try
            {
                var tweet = new TweetDTO()
                {
                    TweetID = "1",
                    UserName = "Rense",
                    Message = "Het is woensdag mijn kamaraden",
                    TweetDate = new DateTime(1998, 4, 1)

                };
                await _timelineService.UpdateTimeline(UserID, tweet);
                return Ok(tweet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize("moderator")]
        [Route("test/moderator")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateTimeline()
        {
            return Ok("Yaaaaay");
        }
    }
}

