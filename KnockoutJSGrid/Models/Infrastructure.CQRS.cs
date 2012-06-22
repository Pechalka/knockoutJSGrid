using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnockoutJSGrid.Controllers;
using MongoDB.Driver.Linq;


namespace KnockoutJSGrid.Models
{
    public interface IQuery<TItem, TFilter>
    {
        TItem Execute(TFilter filter);
    }

    public class CQRSQuery
    {
        public QueryBulder<TResult> For<TResult>()
        {
            return new QueryBulder<TResult>();
        }

        public QueryBulder<IQueryable<TResult>> ForQueryable<TResult>()
        {
            return new QueryBulder<IQueryable<TResult>>();
        }

        public class QueryBulder<TItem>
        {
            public TItem With<TFilter>(TFilter filter)
            {
                var query = DependencyResolver.Current.GetService<IQuery<TItem, TFilter>>();
                return query.Execute(filter);
            }

            public IQueryable<TItem> AsQueryable()
            {
                return BaseMongoQuery.GetCollection<TItem>().AsQueryable();
            }

            public TItem GetBy(Guid id)
            {
                return BaseMongoQuery.GetCollection<TItem>().FindOneById(id);
            }
        }

    }
}