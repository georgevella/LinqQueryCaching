using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Briefs.DataLayer.Caching.InMem
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        private readonly ConcurrentDictionary<Type, ICache> _caches = new ConcurrentDictionary<Type, ICache>();

        public IList<TModel> GetOrAdd<TModel>(Expression expression, Func<IList<TModel>> queryAction)
        {
            var itemCache = GetCache<TModel>();
            return (IList<TModel>)itemCache.GetOrAdd<TModel>(expression, queryAction);
        }

        public object GetOrAddSingleItem<TModel>(Expression expression, Func<object> queryAction)
        {
            throw new NotImplementedException();
        }

        public TResult GetOrAddSingleItem<TModel, TResult>(Expression expression, Func<TResult> queryAction)
        {
            var itemCache = GetCache<TModel>();
            return (TResult)itemCache.GetOrAdd<TResult>(expression, queryAction);
        }

        private ICache GetCache<TModel>()
        {
            var itemCache = _caches.GetOrAdd(typeof(TModel), type => new InMemoryCache(typeof(TModel)));
            return itemCache;
        }

        public void Invalidate<TModel>()
        {
            var itemCache = GetCache<TModel>();
            itemCache.Invalidate();
        }
    }
}