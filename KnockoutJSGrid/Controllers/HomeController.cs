using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KnockoutJSGrid.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace KnockoutJSGrid.Controllers
{
    public class HomeController : Controller
    {
        public CQRSQuery Query = new CQRSQuery();



        public ActionResult Index(PersonsViewModel defaultViewModel)
        {
            return View(defaultViewModel);
        }


 
        public ActionResult List(Sorting sort, FilterParams filterParams, int pageNumber = 1)
        {
            var onePageOfPersons = Query
                .ForQueryable<Person>()
                .With(filterParams)
              //  .OrderBy(sort)
                .GetPage(pageNumber, 10);

            return Json(onePageOfPersons);
        }
    }

    public interface IDefaultValueFor<out TObject>
    {
        TObject DefaultValue();
    }


    public class DefaultValueStorage : IDefaultValueFor<PersonsViewModel>
    {
        public PersonsViewModel DefaultValue()
        {
            var colors = new[]
                             {
                                 new KeyValuePair<string, string>("1", "Black"),
                                 new KeyValuePair<string, string>("2", "Red"),
                                 new KeyValuePair<string, string>("3", "Green"), 
                             };
            var filter = new FilterParams
                             {
                                 Colors = colors,
                                 SelectedColor = "2"
                             };
            var sort = new Sorting
            {
                Field = "FirstName",
                Distinct = "asc"
            };

            return new PersonsViewModel
            {
                Filter = filter,
                Sort = sort
            };
        }
    }


    public class FindPersonsQuery : BaseMongoQuery<Person, FilterParams>
    {
        protected override IMongoQuery buildQuery(FilterParams filter)
        {
            IMongoQuery query = base.buildQuery(filter);

            if (filter.AgeFrom.HasValue)
                query = Query.And(query, Query.GT("Age", filter.AgeFrom));

            if (filter.AgeTo.HasValue)
                query = Query.And(query, Query.LTE("Age", filter.AgeTo));

            return query;
        }
    }

    public class BaseMongoQuery<TItem, TFilter> : BaseMongoQuery, IQuery<IQueryable<TItem>, TFilter> 
    {
        public virtual IQueryable<TItem> Execute(TFilter filter)
        {
            var query = buildQuery(filter);
            return GetCollection<TItem>().Find(query).AsQueryable();
        }

        protected virtual IMongoQuery buildQuery(TFilter filter)
        {
            return Query.Exists("_id", true);
        }
    }

    public class BaseMongoQuery
    {
        public static MongoCollection<TCollection> GetCollection<TCollection>(string collectionName = null)
        {
            var server = MongoServer.Create();
            var database = server.GetDatabase("Persons");

            if (collectionName == null)
                collectionName = typeof(TCollection).Name + "s";
            return database.GetCollection<TCollection>(collectionName);
        }
    }




}

