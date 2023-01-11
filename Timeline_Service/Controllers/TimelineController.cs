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
    //[Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimelineController : Controller
    {
        private readonly TimelineService _timelineService;
        public TimelineController(TimelineService timelineService)
        {
            _timelineService = timelineService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TweetDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<TweetDTO>>> GetTimeline()
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _timelineService.GetTimeline(userId);
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

        //[Authorize("moderator")]
        [Route("test/moderator/{userid}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateTimeline(string userid)
        {
            try
            {
                var result = await _timelineService.GetTimeline(userid);
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
    }
}

