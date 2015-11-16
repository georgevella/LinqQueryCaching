using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Experiments.LinqQueryCaching.Caching
{
    internal class CachedQueryable<TModel> : IQueryable<TModel>
    {
        private readonly IQueryable<TModel> _queryable;
        private readonly ICacheProvider _cacheProvider;
        private readonly IQueryProvider _provider;

        public CachedQueryable(IQueryable<TModel> queryable, ICacheProvider cacheProvider)
        {
            _queryable = queryable;
            _cacheProvider = cacheProvider;
            _provider = new CachedQueryProvider<TModel>(_queryable.Provider, cacheProvider);
        }

        public IEnumerator<TModel> GetEnumerator()
        {
            //Console.WriteLine("Hashcode: {0} [{1}]", _queryable.Expression.GetHashCode(), _queryable.Expression.ToString());
            return _cacheProvider.GetOrAdd(_queryable.Expression, () => _queryable.ToList()).GetEnumerator();
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

    internal class CachedQueryable<TModel, TResult> : IQueryable<TResult>, IOrderedQueryable<TResult>
    {
        private readonly IQueryable<TResult> _queryable;
        private readonly ICacheProvider _cacheProvider;
        private readonly IQueryProvider _provider;

        public CachedQueryable(IQueryable<TResult> queryable, ICacheProvider cacheProvider)
        {
            _queryable = queryable;
            _cacheProvider = cacheProvider;
            _provider = new CachedQueryProvider<TModel>(_queryable.Provider, cacheProvider);
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            //Console.WriteLine("Hashcode: {0} [{1}]", _queryable.Expression.GetHashCode(), _queryable.Expression.ToString());
            return _cacheProvider.GetOrAdd(_queryable.Expression, () => _queryable.ToList()).GetEnumerator();
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

    internal class CachedQueryProvider<TModel> : IQueryProvider
    {
        private readonly IQueryProvider _provider;
        private readonly ICacheProvider _cacheProvider;

        public CachedQueryProvider(IQueryProvider provider, ICacheProvider cacheProvider)
        {
            _provider = provider;
            _cacheProvider = cacheProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return _provider.CreateQuery(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new CachedQueryable<TModel, TElement>(_provider.CreateQuery<TElement>(expression), _cacheProvider);
        }


        public object Execute(Expression expression)
        {
            return _cacheProvider.GetOrAddSingleItem<TModel>(expression, () => _provider.Execute(expression));
        }

        public TResult Execute<TResult>(Expression expression)
        {
            //throw new NotImplementedException();
            return _cacheProvider.GetOrAddSingleItem<TModel, TResult>(expression, () => _provider.Execute<TResult>(expression));
        }
    }
}