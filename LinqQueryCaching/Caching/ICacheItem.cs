using System.Collections.Generic;

namespace Experiments.LinqQueryCaching.Caching
{
    internal interface ICacheItem
    {
        IList<TItem> Get<TItem>();
        TItem GetSingle<TItem>();
    }
}