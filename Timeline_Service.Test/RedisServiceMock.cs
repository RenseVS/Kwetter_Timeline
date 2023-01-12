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

        public async Task<string> GetCacheValueAsync(string key)
        {
            switch (key)
            {
                case "Date1":
                    return await Task.FromResult(new DateTime(1998, 4, 1).ToString()); ;
                case "Date2":
                    return await Task.FromResult(DateTime.Now.ToString());
                default:
                    return await Task.FromResult("dfdfdfd"); ;
            }
        }

        public async Task<IEnumerable<RedisValue>> GetSortedListCacheValueAsync(string key)
        {
            switch (key)
            {
                case "1":
                    var redislist1 = new List<RedisValue>() { new RedisValue("{\"tweetID\":\"abc123\",\"userName\":\"JohnDoe\",\"message\":\"Hello,world!\",\"tweetDate\":\"2022-12-14T12:00:00\"}") };
                    return await Task.FromResult(redislist1);
                case "2":
                    var redislist3 = new List<RedisValue>() { new RedisValue("{\"tweetID\":\"abc123\",\"userName\":\"JohnDoe\",\"message\":\"Hello,world!\",\"tweetDate\":\"2022-12-14T12:00:00\"}") };
                    return await Task.FromResult(redislist3);
                case "3":
                    var redislist2 = new List<RedisValue>() { "dcdvdvd" };
                    return await Task.FromResult(redislist2);
                default:
                    var redislistDefault = new List<RedisValue>() { new RedisValue("{\"tweetID\":\"abc123\",\"userName\":\"JohnDoe\",\"message\":\"Hello,world!\",\"tweetDate\":\"2022-12-14T12:00:00\"}") };
                    return await Task.FromResult(redislistDefault); ;
            }
        }

        public async Task SetCacheValueAsync(string key, string value)
        {
            
        }

        public async Task SetSortedListCacheValueAsync(string key, string value, long seconds)
        {
            
        }
    }
}

