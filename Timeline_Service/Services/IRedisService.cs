using System;
using StackExchange.Redis;

namespace Timeline_Service.Services
{
	public interface IRedisService
	{
        public Task SetSortedListCacheValueAsync(string key, string value, long seconds);
        public Task<IEnumerable<RedisValue>> GetSortedListCacheValueAsync(string key);

        public Task<string> GetCacheValueAsync(string key);
        public Task SetCacheValueAsync(string key, string value);
    }
}

