using Redis.DTOs;

namespace Redis.Service;

public interface IRedisService
{
    Task<List<Item>> GetAllUsingRedis();
}
