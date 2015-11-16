using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Experiments.LinqQueryCaching
{
    public class CachedRepository
    {
        private MemoryCache _cache;

        static class Store<T>
        {
            public static readonly List<T> Items = new List<T>();                        
        }

        public CachedRepository()
        {
            _cache = new MemoryCache("test");
        }

        public T Add<T>(T item)
        {
            Store<T>.Items.Add(item);

            return item;
        }

        public IQueryable<T> All<T>()
        {
            return new CachedQueryable<T>(Store<T>.Items.AsQueryable(), _cache);
        }
    }
}