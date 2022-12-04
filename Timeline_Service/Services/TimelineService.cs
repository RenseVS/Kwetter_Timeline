using System;
using AutoMapper;
using Timeline_Service.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;
using Timeline_Service.Entities;
using static StackExchange.Redis.Role;

namespace Timeline_Service.Services
{
	public class TimelineService
	{
		private readonly IMapper _mapper;
		private readonly RedisService _redisService;
		public TimelineService(IMapper mapper, RedisService redisService)
		{
			_mapper = mapper;
			_redisService = redisService; 
		}

		public async Task<IEnumerable<TweetDTO>> GetTimeline(string userid)
		{
			var cachedvalue = await _redisService.GetCacheValueAsync(userid);
			IEnumerable<Tweet>? timeline;
            try
			{
                timeline = JsonSerializer.Deserialize<IEnumerable<Tweet>>(cachedvalue);
            }
			catch {
                var newtimeline = await CreateNewTimelineForUser(userid);
                return _mapper.Map<IEnumerable<Tweet>, IEnumerable<TweetDTO>>(newtimeline);
            }
            return _mapper.Map<IEnumerable<Tweet>, IEnumerable<TweetDTO>>(timeline).OrderByDescending(tweet => tweet.TweetDate).ToList();
        }

        public async Task UpdateTimeline(string userid, TweetDTO tweet)
        {
			IEnumerable<TweetDTO> Timeline = await GetTimeline(userid);
            List<TweetDTO> TimelineList = Timeline.OrderByDescending(tweet => tweet.TweetDate).ToList();
            if (TimelineList.Count() > 9)
			{
				TimelineList.RemoveRange(9, TimelineList.Count() - 9);
            }
			TimelineList.Insert(0, tweet);
			var newtimeline = _mapper.Map<IEnumerable<TweetDTO>, IEnumerable<Tweet>>(TimelineList);
            string jsonString = JsonSerializer.Serialize(newtimeline);
            await _redisService.SetCacheValueAsync(userid, jsonString);
        }

		public async Task<IEnumerable<Tweet>> CreateNewTimelineForUser(string userid) {
			List<Tweet> timeline = new List<Tweet>();
			for (int i = 0; i < 8; i++) {
				timeline.Add(new Tweet()
				{
					TweetID = "0",
					UserName = "Kwetter inc.",
					Message = "Welcome to Kwetter, When you start following more people your timeline will fill up with interesting messages. PS, im a teapot",
					TweetDate = new DateTime(1998, 4, 1)

				});
			}
            string jsonString = JsonSerializer.Serialize(timeline);
            await _redisService.SetCacheValueAsync(userid, jsonString);
			return timeline;
        }
    }
}

