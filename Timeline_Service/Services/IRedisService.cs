using System;
using StackExchange.Redis;

namespace Timeline_Service.Services
{
	public interface IRedisService
	{
        public Task SetSortedListCacheValueAsync(string key, string value, long seconds);
        public Task<IEnumerable<RedisValue>> GetSortedListCacheValueAsync(string key);
    }
}

