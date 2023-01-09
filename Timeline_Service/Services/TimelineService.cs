using System;
using AutoMapper;
using Timeline_Service.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;
using Timeline_Service.Entities;
using static StackExchange.Redis.Role;
using static System.Net.Mime.MediaTypeNames;
using StackExchange.Redis;

namespace Timeline_Service.Services
{
	public class TimelineService
	{
		private readonly IMapper _mapper;
		private readonly IRedisService _redisService;
		public TimelineService(IMapper mapper, IRedisService redisService)
		{
			_mapper = mapper;
			_redisService = redisService; 
		}

		public async Task<IEnumerable<TweetDTO>> GetTimeline(string userid)
		{
			var cachedvalue = await _redisService.GetSortedListCacheValueAsync(userid);
			IEnumerable<Tweet> timeline;
            try
			{
                timeline = await GetTimelineFromCache(userid);
            }
			catch {

                timeline = await GetTimelineFromDatabase(userid);
            }
            return _mapper.Map<IEnumerable<Tweet>, IEnumerable<TweetDTO>>(timeline).OrderByDescending(tweet => tweet.TweetDate).ToList();
        }

        public async Task UpdateTimeline(string userid, TweetDTO tweet)
        {
            string jsonString = JsonSerializer.Serialize(_mapper.Map<TweetDTO, Tweet>(tweet));
            await _redisService.SetSortedListCacheValueAsync(userid, jsonString, tweet.TweetDate.Ticks);
        }

		//public async Task<IEnumerable<Tweet>> CreateNewTimelineForUser(string userid) {
  //          Tweet tweet = new Tweet()
  //          {
  //              TweetID = "0",
  //              UserName = "Kwetter inc.",
  //              Message = "Welcome to Kwetter, When you start following more people your timeline will fill up with interesting messages. PS, im a teapot",
  //              TweetDate = new DateTime(1998, 4, 1)

  //          };
  //          string jsonString = JsonSerializer.Serialize(tweet);
  //          for (int i = 0; i < 8; i++) {
  //              await _redisService.SetSortedListCacheValueAsync(userid, jsonString, 0);
		//	}
		//	return timeline;
  //      }

        public async Task<IEnumerable<Tweet>> GetTimelineFromCache(string userid) {
            var cachedvalue = await _redisService.GetSortedListCacheValueAsync(userid);
            return cachedvalue.Select(x => JsonSerializer.Deserialize<Tweet>(x)).ToList();
        }

        public async Task<IEnumerable<Tweet>> GetTimelineFromDatabase(string userid)
        {
            Tweet tweet = new Tweet()
            {
                TweetID = "0",
                UserName = "Kwetter inc.",
                Message = "Welcome to Kwetter, When you start following more people your timeline will fill up with interesting messages. PS, im a teapot",
                TweetDate = new DateTime(1998, 4, 1)

            };
            return new List<Tweet>() { tweet };
        }
    }
}

