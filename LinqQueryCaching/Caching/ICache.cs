using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Briefs.DataLayer.Caching
{
    internal interface ICache
    {
        IList<T> GetOrAdd<T>(Expression expression, Func<IList<T>> queryAction);
        T GetOrAdd<T>(Expression expression, Func<T> queryAction);
        void Invalidate();
    }
}