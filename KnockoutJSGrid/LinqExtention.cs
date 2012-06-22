using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using KnockoutJSGrid.Models;

namespace KnockoutJSGrid
{
    public static class LinqExtention
    {

    #region sorting 
                public static IQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> source, Sorting sort) where TEntity : class
                {
                    string sortExp = string.Format("{0} {1}", sort.Field, sort.Distinct.ToUpper());
                    return source.OrderBy(sortExp);
                }

        /// <summary>
        /// Extends method which allow to sort by string field name.
        /// Allow to use a relative object definition for sorting (ex:LinkedObject.FieldsName1)
        /// </summary>
        /// <typeparam name="TEntity">Current Object type for query</typeparam>
        /// <param name="source">list of defined object</param>
        /// <param name="sortExpression">string name of the field we want to sort by</param>
        /// <returns>Query sorted by sortExpression</returns>
        public static IQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            var type = typeof (TEntity);
            // Remember that for ascending order GridView just returns the column name and
            // for descending it returns column name followed by DESC keyword
            // Therefore we need to examine the sortExpression and separate out Column Name and
            // order (ASC/DESC)
            string[] expressionParts = sortExpression.Split(' ');
                // Assuming sortExpression is like [ColumnName DESC] or [ColumnName]
            string orderByProperty = expressionParts[0];
            string sortDirection = "ASC";
            string methodName = "OrderBy";
            //if sortDirection is descending
            if (expressionParts.Length > 1 && expressionParts[1] == "DESC")
            {
                sortDirection = "Descending";
                methodName += sortDirection; // Add sort direction at the end of Method name
            }
            MethodCallExpression resultExp = null;
            if (!orderByProperty.Contains("."))
            {
                var property = type.GetProperty(orderByProperty);
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof (Queryable), methodName,
                                            new Type[] {type, property.PropertyType},
                                            source.Expression, Expression.Quote(orderByExp));
            }
            else
            {
                Type relationType = type.GetProperty(orderByProperty.Split('.')[0]).PropertyType;
                PropertyInfo relationProperty = type.GetProperty(orderByProperty.Split('.')[0]);
                PropertyInfo relationProperty2 = relationType.GetProperty(orderByProperty.Split('.')[1]);
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, relationProperty);
                var propertyAccess2 = Expression.MakeMemberAccess(propertyAccess, relationProperty2);
                var orderByExp = Expression.Lambda(propertyAccess2, parameter);
                resultExp = Expression.Call(typeof (Queryable), methodName,
                                            new Type[] {type, relationProperty2.PropertyType},
                                            source.Expression, Expression.Quote(orderByExp));
            }
            return source.Provider.CreateQuery<TEntity>(resultExp);
        }

        /// <summary>
        /// Allow to add another sorting on a query with a string representation of the field to sort by.
        /// </summary>
        /// <typeparam name="TEntity">Current Object type for query</typeparam>
        /// <param name="source">list of defined object</param>
        /// <param name="sortExpression">string name of the field we want to sort by</param>
        /// <returns>Query sorted by sortExpression</returns>
        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(
            this IOrderedQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            var type = typeof (TEntity);
            // Remember that for ascending order GridView just returns the column name and
            // for descending it returns column name followed by DESC keyword
            // Therefore we need to examine the sortExpression and separate out Column Name and
            // order (ASC/DESC)
            string[] expressionParts = sortExpression.Split(' ')
                ; // Assuming sortExpression is like [ColumnName DESC] or [ColumnName]
            string orderByProperty = expressionParts[0];
            string sortDirection = "ASC";
            string methodName = "ThenBy";
            //if sortDirection is descending
            if (expressionParts.Length > 1 && expressionParts[1] == "DESC")
            {
                sortDirection = "Descending";
                methodName += sortDirection; // Add sort direction at the end of Method name
            }
            MethodCallExpression resultExp = null;
            if (!orderByProperty.Contains("."))
            {
                var property = type.GetProperty(orderByProperty);
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof (Queryable), methodName,
                                            new Type[] {type, property.PropertyType},
                                            source.Expression, Expression.Quote(orderByExp));
            }
            else
            {
                Type relationType = type.GetProperty(orderByProperty.Split('.')[0]).PropertyType;
                PropertyInfo relationProperty = type.GetProperty(orderByProperty.Split('.')[0]);
                PropertyInfo relationProperty2 = relationType.GetProperty(orderByProperty.Split('.')[1]);
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, relationProperty);
                var propertyAccess2 = Expression.MakeMemberAccess(propertyAccess, relationProperty2);
                var orderByExp = Expression.Lambda(propertyAccess2, parameter);
                resultExp = Expression.Call(typeof (Queryable), methodName,
                                            new Type[] {type, relationProperty2.PropertyType},
                                            source.Expression, Expression.Quote(orderByExp));
            }
            return (IOrderedQueryable<TEntity>) source.Provider.CreateQuery<TEntity>(resultExp);
        }
    #endregion

    #region paging
        public static IQueryable<TItem> Paging<TItem>(this IQueryable<TItem> source, int pageNumber, int pageSize, out int totalItemsCount)
        {
            totalItemsCount = source.Count();

            return source
                .Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static PageOf<TItem> GetPage<TItem>(this IQueryable<TItem> source, int pageNumber, int pageSize)
        {
            int totalItemsCount;
            var items = source.Paging(pageNumber, pageSize, out totalItemsCount).ToList();
            var paging = new Paging
            {
                TotalItemsCount = totalItemsCount,
                PageNumber = pageNumber
            };

            return new PageOf<TItem>
            {
                Data = items,
                Paging = paging
            };
        }
    #endregion

    }
}