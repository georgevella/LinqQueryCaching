using System.Linq;

namespace Experiments.LinqQueryCaching
{
    public interface IRepository
    {
        TModel Find<TModel>(object id) where TModel : class;
        IQueryable<TModel> All<TModel>() where TModel : class;
        void Add<TModel>(TModel item) where TModel : class, new();
        void AddOrUpdate<TModel>(TModel item) where TModel : class, new();
        void Save(bool validate = false);

        void Remove<TModel>(TModel model) where TModel : class, new();
    }
}