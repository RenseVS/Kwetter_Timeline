using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Timeline_Service.Services
{
	public class RedisService : IRedisService
	{
		private readonly IConnectionMultiplexer _connectionMultiplexer;
		public RedisService(IConnectionMultiplexer connectionMultiplexer)
		{
			_connectionMultiplexer = connectionMultiplexer;
		}

		public async Task<string> GetCacheValueAsync(string key)
		{
			var db = _connectionMultiplexer.GetDatabase();
			return await db.StringGetAsync(key);
		}

        public async Task SetCacheValueAsync(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async Task SetSortedListCacheValueAsync(string key, string value, long seconds)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.SortedSetRemoveRangeByRankAsync(key, 10, 9999);
            await db.SortedSetAddAsync(key, value, seconds);
        }

        public async Task<IEnumerable<RedisValue>> GetSortedListCacheValueAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.SortedSetRangeByRankAsync(key);
        }
    }
}

