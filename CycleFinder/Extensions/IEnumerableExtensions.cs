using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> e, int? limit) => !limit.HasValue ? e : Enumerable.TakeLast(e, limit.Value);

        public static async Task<TResult[]> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return await Task.WhenAll(source.Select(selector));
        }

        public static async Task<Dictionary<TKey, TResult>> ToResults<TKey, TResult>(this IEnumerable<KeyValuePair<TKey, Task<TResult>>> input)
        {
            var pairs = await Task.WhenAll
            (
                input.Select
                (
                    async pair => new { pair.Key, Value = await pair.Value }
                )
            );
            return pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

    }
}
