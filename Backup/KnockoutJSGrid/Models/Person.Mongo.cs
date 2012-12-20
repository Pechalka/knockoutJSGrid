using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace KnockoutJSGrid.Models
{
    public class PersonsGenerator
    {
        #region Initialization

        private static readonly Dictionary<string, Gender> FirstNames = new Dictionary<string, Gender>
                                                                            {
                                                                                {"Roman", Gender.Male},
                                                                                {"Vitaliy", Gender.Male},
                                                                                {"Alexandr", Gender.Male},
                                                                                {"Vadim", Gender.Male},
                                                                                {"Luiza", Gender.Female},
                                                                                {"Marina", Gender.Female},
                                                                                {"Elena", Gender.Female},
                                                                                {"Anton", Gender.Male},
                                                                                {"Vladimir", Gender.Male},
                                                                                {"Alexey", Gender.Male}
                                                                            };

        private static readonly string[] LastNames = new[]
        {
            "Gomolko",
            "Koval",
            "Korotkiy",
            "Kurachevskiy",
            "Chumakov",
            "Alieva",
            "Basyuk",
            "Plutalov",
            "Grankin",
        };

        private static readonly Random Random = new Random();

        public static IList<Person> Generate()
        {
            var persons = new List<Person>();
            for (int i = 0; i < 100; i++)
            {
                var firstNameAndGender = FirstNames.Random();
                var lastName = LastNames.Random();
                var age = Random.Next(18, 45);

                var person = new Person(firstNameAndGender.Key, lastName, age, firstNameAndGender.Value);
                persons.Add(person);
            }
            return persons;
        }
        #endregion Initialization

        public static void MakeTestData()
        {
            var server = MongoServer.Create(Configuration.DataBaseConnectionString);
            var database = server.GetDatabase("Persons");
            var collection = database.GetCollection<Person>("Persons");
            collection.RemoveAll();
            var persons = Generate();
            collection.InsertBatch(persons);
        }
    }
}