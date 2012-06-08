using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace KnockoutJSGrid.Models
{
    public class Entity
    {
        public Guid Id { get; set; }
    }

    public class MongoRepository<T> where T : Entity
    {
        private readonly MongoCollection<T> _collection;

        public MongoRepository(string databaseName, string collectionName)
        {
            var server = MongoServer.Create();
            var database = server.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collectionName);
        }


        protected IQueryable<T> Items
        {
            get { return _collection.AsQueryable(); }
        }

        public void Save(T obj)
        {
            _collection.Save(obj);
        }
    }
}