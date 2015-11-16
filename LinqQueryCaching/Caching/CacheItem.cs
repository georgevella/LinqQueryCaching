using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Experiments.LinqQueryCaching.Caching
{
    internal class CacheItem<T> : ICacheItem
    {
        private readonly long _timeStamp;
        private byte[] _itemData;

        public CacheItem(IEnumerable<T> items)
        {
            _timeStamp = Stopwatch.GetTimestamp();
            Store(new List<T>(items));
        }

        public CacheItem(T item)
        {
            _timeStamp = Stopwatch.GetTimestamp();
            Store(new List<T>() { item });
        }

        private void Store(List<T> listOfT)
        {
            _itemData = CacheItemSerializationHelper.Serialize(listOfT);
        }

        private List<T> GetImpl()
        {
            return CacheItemSerializationHelper.Deserialize<T>(_itemData);
        }

        public IList<TItem> Get<TItem>()
        {
            return (IList<TItem>)GetImpl();
        }

        public TItem GetSingle<TItem>()
        {
            return (TItem)GetImpl().Cast<TItem>().FirstOrDefault();
        }
    }
}