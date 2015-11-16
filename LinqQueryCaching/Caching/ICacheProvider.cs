using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Briefs.DataLayer.Caching
{
    public interface ICacheProvider
    {
        IList<TModel> GetOrAdd<TModel>(Expression expression, Func<IList<TModel>> queryAction);
        object GetOrAddSingleItem<TModel>(Expression expression, Func<object> queryAction);
        TResult GetOrAddSingleItem<TModel, TResult>(Expression expression, Func<TResult> queryAction);

        void Invalidate<TModel>();
    }
}