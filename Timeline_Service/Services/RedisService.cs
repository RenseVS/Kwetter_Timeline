﻿using System;
using StackExchange.Redis;

namespace Timeline_Service.Services
{
	public class RedisService
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
    }
}

