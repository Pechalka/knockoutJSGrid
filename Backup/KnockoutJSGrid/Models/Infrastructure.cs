using System;
using System.Collections.Generic;
using System.Linq;

namespace KnockoutJSGrid.Models
{
    public class Paging
    {
        public Paging(int page, int totalItemsCount, int pageSize = 10)
        {
            PageNumber = page;
            TotalItemsCount = totalItemsCount;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int TotalPagesCount {
            get { return (int)Math.Ceiling(1D * TotalItemsCount / PageSize); }
        }

        public  int PageSize;
        public int TotalItemsCount { get; set; }
    }


    public class PageOf<T>
    {
        public IEnumerable<T> Data { get; set; }

        public Paging Paging { get; set; }
    }

    /// <summary>
    /// Provides a set of static methods for querying objects that implement IEnumerable&lt;T&gt;.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Randomizer instance for <see cref="Random()"/> method
        /// </summary>
        private static readonly Random Randomizer = new Random();

        /// <summary>
        /// Get random item from specified collection.
        /// </summary>
        /// <typeparam name="T">Type of collection elements'</typeparam>
        /// <param name="collection">The collection to get default element from.</param>
        /// <returns>Random element of collection. <c>null</c> if collection is empty</returns>
        public static T Random<T>(this IEnumerable<T> collection)
        {
            if (collection.Any() == false)
            {
                return default(T);
            }
            int elementIndex = Randomizer.Next(0, collection.Count());
            return collection.ElementAt(elementIndex);
        }
    }


    public class Sorting
    {
        public string Field { get; set; }
        public string Distinct { get; set; } // asc desc
    }



    public class Entity
    {
        public Guid Id { get; set; }
    }
}