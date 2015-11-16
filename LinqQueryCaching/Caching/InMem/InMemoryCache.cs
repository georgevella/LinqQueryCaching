using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Briefs.DataLayer.Caching.InMem
{
    internal class InMemoryCache : ConcurrentDictionary<string, ICacheItem>, ICache
    {
        private readonly Type _model;

        public InMemoryCache(Type model)
        {
            _model = model;
        }

        public IList<T> GetOrAdd<T>(Expression expression, Func<IList<T>> queryAction)
        {
            expression = Evaluator.PartialEval(expression);
            expression = LocalCollectionExpander.Rewrite(expression);
            var key = expression.ToString();

            var cachedItem = base.GetOrAdd(key, s => new CacheItem<T>(queryAction()));
            return cachedItem.Get<T>();
        }

        public T GetOrAdd<T>(Expression expression, Func<T> queryAction)
        {
            expression = Evaluator.PartialEval(expression);
            expression = LocalCollectionExpander.Rewrite(expression);
            var key = expression.ToString();

            var cachedItem = base.GetOrAdd(key, s => new CacheItem<T>(queryAction()));
            return cachedItem.GetSingle<T>();
        }

        public void Invalidate()
        {
            Clear();
        }
    }
}