using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.DTOs;
using Redis.Service;
using System.Text;

namespace Redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly ILogger<RedisController> _logger;
        private readonly IRedisService _redisService;

        public RedisController(
            ILogger<RedisController> logger,
            IDistributedCache cache)
        {
            _logger = logger;
            _redisService = new RedisService(cache);
        }
        
        [HttpGet("GetAllUsingRedis")]
        public async Task<IActionResult> GetAllUsingRedis()
        {
            var response = await _redisService.GetAllUsingRedis();

            return Ok(response);
        }
    }
}