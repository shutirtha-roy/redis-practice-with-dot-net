using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.Controllers;
using Redis.DTOs;
using System.Text;

namespace Redis.Service;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _cache;
    private const string KEY = "Get_ALL";

    public RedisService(IDistributedCache cache)
        => _cache = cache;

    public async Task<List<Item>> GetAllUsingRedis()
    {
        var items = new List<Item>();

        var cachedData = await _cache.GetAsync(KEY);

        if (cachedData is not null)
        {
            var oldCachedDataString = Encoding.UTF8.GetString(cachedData);
            items = JsonConvert.DeserializeObject<List<Item>>(oldCachedDataString);

            return items;
        }

        items = new List<Item>()
        {
            new Item
            {
                Id = 1,
                Description = "Example Item 1",
                Name= "Name",
                Total= 1,
                Type = "New"
            }
        };

        //serialize data
        var cachedDataString = JsonConvert.SerializeObject(items);
        var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);

        //set cache options
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(2))
            .SetSlidingExpiration(TimeSpan.FromMinutes(1));

        //Add data to cache
        await _cache.SetAsync(KEY, newDataToCache, options);

        return items;
    }
}
