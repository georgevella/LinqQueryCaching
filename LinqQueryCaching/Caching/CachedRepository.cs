using System;
using System.Linq;
using Experiments.LinqQueryCaching;


namespace Briefs.DataLayer.Caching
{
    public class CachedRepository : IRepository
    {
        private readonly ICacheProvider _cacheProvider;
        //private readonly Repository _innerRepository;

        public CachedRepository(ICacheProvider cacheProvider/*, DbContext dbContext*/)
        {
            _cacheProvider = cacheProvider;
            //_innerRepository = new Repository(dbContext);
        }

        //protected internal Repository InnerRepository
        //{
        //    get { return _innerRepository; }
        //}

        public TModel Find<TModel>(object id) where TModel : class
        {
            //return _innerRepository.Find<TModel>(id);
            throw new NotImplementedException();
        }

        public IQueryable<TModel> All<TModel>() where TModel : class
        {
            return new CachedQueryable<TModel>(Store<TModel>.Items.AsQueryable(), _cacheProvider);
        }

        public void Add<TModel>(TModel item) where TModel : class, new()
        {
            Store<TModel>.Items.Add(item);
            //_innerRepository.Add<TModel>(item);
        }

        public void AddOrUpdate<TModel>(TModel item) where TModel : class, new()
        {
            //_innerRepository.AddOrUpdate<TModel>(item);
            throw new NotImplementedException();
        }

        public void Save(bool validate = false)
        {
            //_innerRepository.Save(validate);
        }

        //public EagerLoader EagerLoading()
        //{
        //    //return _innerRepository.EagerLoading();
        //    throw new NotImplementedException();
        //}

        public void Remove<TModel>(TModel model) where TModel : class, new()
        {
            //_innerRepository.Remove<TModel>(model);
        }
    }
}