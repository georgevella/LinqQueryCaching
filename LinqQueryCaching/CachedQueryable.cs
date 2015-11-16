using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace Experiments.LinqQueryCaching
{
    public class CachedQueryable<T> : IQueryable<T>
    {
        class CachedQueryProvider : IQueryProvider
        {
            private readonly IQueryProvider _provider;
            private MemoryCache _cache;

            public CachedQueryProvider(IQueryProvider provider, MemoryCache cache)
            {
                _provider = provider;
                _cache = cache;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return _provider.CreateQuery(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new CachedQueryable<TElement>(_provider.CreateQuery<TElement>(expression), _cache);
            }

            public object Execute(Expression expression)
            {
                return _provider.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _provider.Execute<TResult>(expression);
            }
        }

        private readonly IQueryable<T> _queryable;
        private readonly MemoryCache _cache;
        private readonly IQueryProvider _provider;

        public CachedQueryable(IQueryable<T> queryable, MemoryCache cache)
        {
            _queryable = queryable;
            _cache = cache;
            _provider = new CachedQueryProvider(_queryable.Provider, _cache);
        }

        public IEnumerator<T> GetEnumerator()
        {
            //Console.WriteLine("Hashcode: {0} [{1}]", _queryable.Expression.GetHashCode(), _queryable.Expression.ToString());
            var key = _queryable.Expression.ToString();

            var result = _cache[key] as List<T>;
            if (result != null)
                return result.GetEnumerator();

            var items = _queryable.ToList();
            _cache.Add(key, items, DateTimeOffset.MaxValue);

            return _queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression => _queryable.Expression;
        public Type ElementType => _queryable.ElementType;

        public IQueryProvider Provider
        {
            get
            {
                return _provider;
            }
        }
    }
}