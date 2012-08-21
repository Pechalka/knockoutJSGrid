using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace KnockoutJSGrid.Models
{


    public interface IQuery<T>
    {
        IQuery<T> FindBy<TFilter>(TFilter filter);
        IQuery<T> Set(Sorting sorting);
        IQuery<T> Page(int pageNumber);
        IQuery<T> Limite(int pageSize);
        PageOf<T> Execute();
    }


    public class MongoQuery<T> : IQuery<T>
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private IMongoQuery _query;
        private Sorting _sorting;

        public IQuery<T> FindBy<TParams>(TParams filter)
        {
            var translator = DependencyResolver.Current.GetService<ITranclator<TParams, IMongoQuery>>();
            _query = translator.Tranclate(filter);

            return this;
        }

        public IQuery<T> Set(Sorting sorting)
        {
            _sorting = sorting;
            return this;
        }

        public IQuery<T> Page(int pageNumber)
        {
            _pageNumber = pageNumber;
            return this;
        }

        public IQuery<T> Limite(int pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        public PageOf<T> Execute()
        {
            var collection = GetCollection<T>();

            var sortBy = _sorting.Distinct == "asc" ? SortBy.Ascending(_sorting.Field) : SortBy.Descending(_sorting.Field);

            var totalItemCount = (int)collection.Find(_query).Count();
            var paging = new Paging(_pageNumber, totalItemCount, _pageSize);

            var items = collection.Find(_query).SetSortOrder(sortBy).SetSkip((_pageNumber - 1) * _pageSize).SetLimit(_pageSize);

            return new PageOf<T>
            {
                Data = items,
                Paging = paging
            };
        }

        private MongoCollection<TCollection> GetCollection<TCollection>(string collectionName = null)
        {
            var server = MongoServer.Create(Configuration.DataBaseConnectionString);
            var database = server.GetDatabase("Persons");

            if (collectionName == null)
                collectionName = typeof(TCollection).Name + "s";
            return database.GetCollection<TCollection>(collectionName);
        }
    }

    public interface ITranclator<in TInput, out TResult>
    {
        TResult Tranclate(TInput input);
    }


    public class PersonTraslator : ITranclator<FilterParams, IMongoQuery>
    {
        public IMongoQuery Tranclate(FilterParams filter)
        {
            IMongoQuery query = Query.Exists("_id");

            if (filter.AgeFrom.HasValue)
                query = Query.And(query, Query.GT("Age", filter.AgeFrom));

            if (filter.AgeTo.HasValue)
                query = Query.And(query, Query.LTE("Age", filter.AgeTo));

            return query;
        }
    }
}