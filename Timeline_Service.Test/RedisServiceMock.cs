using System;
using System.Linq.Expressions;
using StackExchange.Redis;
using Timeline_Service.Services;

namespace Timeline_Service.Test
{
	public class RedisServiceMock : IRedisService
	{
		public RedisServiceMock()
		{
		}

        public async Task<IEnumerable<RedisValue>> GetSortedListCacheValueAsync(string key)
        {
            switch (key)
            {
                case "1":
                    var redislist1 = new List<RedisValue>() { new RedisValue("{\"TweetID\":\"abc123\",\"UserName\":\"JohnDoe\",\"Message\":\"Hello,world!\",\"TweetDate\":\"2022-12-14T12:00:00\"}") };
                    return await Task.FromResult(redislist1);
                case "2":
                    var redislist2 = new List<RedisValue>() { };
                    return await Task.FromResult(redislist2);
                case "3":
                    var redislist3 = new List<RedisValue>() { new RedisValue("{\"TwetID\":\"abc123\",\"Userame\":\"JohnDoe\",\"Message\":\"Hello,world!\",\"TweetDate\":\"2022-12-14T12:00:00\"}") };
                    return await Task.FromResult(redislist3);
                default:
                    var redislistDefault = new List<RedisValue>() { new RedisValue("{\"TweetID\":\"abc123\",\"UserName\":\"JohnDoe\",\"Message\":\"Hello,world!\",\"TweetDate\":\"2022-12-14T12:00:00\"}") };
                    return await Task.FromResult(redislistDefault); ;
            }
        }

        public Task SetSortedListCacheValueAsync(string key, string value, long seconds)
        {
            throw new NotImplementedException();
        }
    }
}

