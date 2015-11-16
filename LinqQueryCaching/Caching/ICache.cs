using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Experiments.LinqQueryCaching.Caching
{
    internal interface ICache
    {
        IList<T> GetOrAdd<T>(Expression expression, Func<IList<T>> queryAction);
        T GetOrAdd<T>(Expression expression, Func<T> queryAction);
        void Invalidate();
    }
}