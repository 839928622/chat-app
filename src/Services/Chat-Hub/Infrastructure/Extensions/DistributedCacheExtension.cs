using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Extensions
{
    public static class DistributedCacheExtension
    {
       
            public static async Task SetRecordAsync<T>(this IDistributedCache cache, string key, T data, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = absoluteExpireTime,
                    SlidingExpiration = slidingExpireTime
                };
                var jsonData = System.Text.Json.JsonSerializer.Serialize(data);
                await cache.SetStringAsync(key, jsonData, options);
            }

            public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string key)
            {
                var jsonData = await cache.GetStringAsync(key);
                return jsonData is null ? 
                    default(T) : 
                    System.Text.Json.JsonSerializer.Deserialize<T>(jsonData,new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    });
            }
        }
	
}
