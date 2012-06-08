using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnockoutJSGrid.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Person : Entity
    {
        public Person()
        {
        }

        public Person(string firstName, string lastName, int age, Gender gender)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.Gender = gender;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
    }

    public class FilterParams
    {
        public FilterParams()
        {
            ShowFemale = true;
            ShowMale = true;
        }

        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool ShowMale { get; set; }
        public bool ShowFemale { get; set; }

        public KeyValuePair<string, string>[] Colors { get; set; }
        public string SelectedColor { get; set; }
    }

    public interface IPersonRepository
    {
        IQueryable<Person> Persons { get; }
        void Save(Person person);
    }
}