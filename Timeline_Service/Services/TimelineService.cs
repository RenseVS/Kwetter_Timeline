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
        private readonly CelebertyService _celebertyService;
		public TimelineService(IMapper mapper, IRedisService redisService, CelebertyService celebertyService)
		{
			_mapper = mapper;
			_redisService = redisService;
            _celebertyService = celebertyService;
		}

		public async Task<IEnumerable<TweetDTO>> GetTimeline(string userid)
		{
			IEnumerable<Tweet> timeline;
            try
			{
                timeline = await GetTimelineFromCache(userid);
            }
			catch {

                timeline = await GetTimelineFromDatabase(userid);
            }
            return _mapper.Map<IEnumerable<Tweet>, IEnumerable<TweetDTO>>(timeline).OrderByDescending(tweet => tweet.tweetDate).ToList();
        }

        public async Task UpdateTimeline(string userid, TweetDTO tweet)
        {
            string jsonString = JsonSerializer.Serialize(_mapper.Map<TweetDTO, Tweet>(tweet));
            await _redisService.SetSortedListCacheValueAsync(userid, jsonString, tweet.tweetDate.Ticks);
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
            TimeSpan ts;
            try
            {
                string lastCelebCheck = await _redisService.GetCacheValueAsync("Date" + userid);
                ts = DateTime.Now - DateTime.Parse(lastCelebCheck);
            }
            catch {
                ts = DateTime.Now - new DateTime(1900);
            }
            
            if (ts.TotalMinutes > 5)
            {
                var tweetlist = cachedvalue.Select(x => JsonSerializer.Deserialize<Tweet>(x)).ToList();
                var celebtimeline = await _celebertyService.GetCelebTweets(userid);
                UpdateTimelineWithCelebs(userid, celebtimeline);

                tweetlist = tweetlist.Concat(celebtimeline).ToList();
                var longlist =  tweetlist.OrderBy(x => x.tweetDate).ToList();
                await _redisService.SetCacheValueAsync("Date" + userid, DateTime.Now.ToString());
                if (longlist.Count > 10)
                {
                    longlist.RemoveRange(10, longlist.Count());
                    return longlist;
                }
                else {
                    return longlist;
                }
            }
            else {
                return cachedvalue.Select(x => JsonSerializer.Deserialize<Tweet>(x)).ToList();
            }
        }

        public async Task UpdateTimelineWithCelebs(string userid, IEnumerable<Tweet> tweets)
        {
            foreach (Tweet tweet in tweets) {
                string jsonString = JsonSerializer.Serialize(tweet);
                await _redisService.SetSortedListCacheValueAsync(userid, jsonString, tweet.tweetDate.Ticks);
            }
        }

        public async Task<IEnumerable<Tweet>> GetTimelineFromDatabase(string userid)
        {
            Tweet tweet = new Tweet()
            {
                tweetID = "0",
                userName = "Kwetter inc.",
                message = "Welcome to Kwetter, When you start following more people your timeline will fill up with interesting messages. PS, im a teapot",
                tweetDate = new DateTime(1998, 4, 1)

            };

            TweetDTO tweetDTO = new TweetDTO()
            {
                tweetID = "0",
                userName = "Kwetter inc.",
                message = "Welcome to Kwetter, When you start following more people your timeline will fill up with interesting messages. PS, im a teapot",
                tweetDate = new DateTime(1998, 4, 1)

            };
            UpdateTimeline(userid, tweetDTO);
            return new List<Tweet>() { tweet };
        }
    }
}

