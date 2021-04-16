using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Microsoft.Extensions.Caching.Distributed
{
    public static class NtityCache
    {
        private static string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Serialize(value, options);
        }

        private static TValue? Deserialize<TValue>(string value, JsonSerializerOptions? options = null)
        {
            return value is not null ? JsonSerializer.Deserialize<TValue>(value, options) : default;
        }

        public static TValue? Get<TValue>(this IDistributedCache cache,
            string key,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var value = cache.GetString(key);
            return Deserialize<TValue>(value, jsonSerializerOptions);
        }

        public static async Task<TValue?> GetAsync<TValue>(this IDistributedCache cache,
            string key,
            JsonSerializerOptions? jsonSerializerOptions = null,
            CancellationToken token = default)
        {
            var value = await cache.GetStringAsync(key, token);
            return Deserialize<TValue>(value, jsonSerializerOptions);
        }

        public static void Set<TValue>(this IDistributedCache cache,
            string key,
            TValue value,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var stringValue = Serialize(value, jsonSerializerOptions);
            cache.SetString(key, stringValue);
        }

        public static void Set<TValue>(this IDistributedCache cache,
            string key,
            TValue value,
            DistributedCacheEntryOptions options,
            JsonSerializerOptions? jsonSerializerOptions = default)
        {
            var stringValue = Serialize(value, jsonSerializerOptions);
            cache.SetString(key, stringValue, options);
        }

        public static async Task SetAsync<TValue>(this IDistributedCache cache,
            string key,
            TValue value,
            JsonSerializerOptions? jsonSerializerOptions = default,
            CancellationToken token = default)
        {
            var stringValue = Serialize(value, jsonSerializerOptions);
            await cache.SetStringAsync(key, stringValue, token);
        }

        public static async Task SetAsync<TValue>(this IDistributedCache cache,
            string key,
            TValue value,
            DistributedCacheEntryOptions options,
            JsonSerializerOptions? jsonSerializerOptions = null,
            CancellationToken token = default)
        {
            var stringValue = Serialize(value, jsonSerializerOptions);
            await cache.SetStringAsync(key, stringValue, options, token);
        }
    }
}