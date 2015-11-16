using System.Collections.Generic;

namespace Briefs.DataLayer.Caching
{
    internal interface ICacheItem
    {
        IList<TItem> Get<TItem>();
        TItem GetSingle<TItem>();
    }
}