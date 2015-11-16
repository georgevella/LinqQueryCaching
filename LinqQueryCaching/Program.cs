using System.Linq;
using System.Runtime.CompilerServices;
using Briefs.DataLayer.Caching;
using Briefs.DataLayer.Caching.InMem;
using Experiments.LinqQueryCaching.Model;

namespace Experiments.LinqQueryCaching
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new InMemoryCacheProvider();
            var r = new CachedRepository(provider);

            r.Add(new Customer(1, "Joe", "Vella"));
            r.Add(new Customer(2, "George", "Vella"));
            r.Add(new Customer(3, "Tatiana", "Camenzuli"));


            var id = GetCustomerId(r, "Joe");
            var id2 = GetCustomerId(r, "George");
            var id3 = GetCustomerId(r, new[] { "George" });
            var id4 = GetCustomerId(r, new[] { "Joe" });

            //var allSalesByJoe1 = query1.Where(x => x.Name == "Joe").ToList();
            //var allSalesByJoe2 = query1.Where(x => x.Name == "George").ToList();            


            //var filter1 = query1.Where(x => x.Value.Contains("t")).Select(x => x.Value);
            //var filter2 = query1.Where(x => x.Value.Contains("t")).Select(x => x.Value);

            //var result1 = filter1.ToList();
            //var result2 = filter2.ToList();


            //var t = filter1.FirstOrDefault();
        }

        private static int GetCustomerId(CachedRepository cachedRepository, string name)
        {
            return cachedRepository.All<Customer>().Where(x => x.Name == name).Select(x => x.Id).FirstOrDefault();
        }
        private static int GetCustomerId(CachedRepository cachedRepository, string[] names)
        {
            return cachedRepository.All<Customer>().Where(x => names.Contains(x.Name)).Select(x => x.Id).FirstOrDefault();
        }
    }
}
