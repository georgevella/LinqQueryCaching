using System.Collections.Generic;

namespace Experiments.LinqQueryCaching
{
    static class Store<T>
    {
        public static readonly List<T> Items = new List<T>();
    }
}