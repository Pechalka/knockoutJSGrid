using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KnockoutJSGrid.Models;

namespace KnockoutJSGrid.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public HomeController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

		public ActionResult Index()
		{
		    var colors = new KeyValuePair<string, string>[]
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

			return View(new PersonsViewModel
			                {
			                    Filter = filter,
                                Sort = sort
			                });
		}

        private Func<Person, bool> CreateSpecification(FilterParams filterParams)//TODO: use Specification pattern, fix filter
        {
            if (filterParams.AgeFrom.HasValue)
            {
                return r => r.Age >= filterParams.AgeFrom;
            }
            if (filterParams.AgeTo.HasValue)
            {
                return r => r.Age <= filterParams.AgeTo;
            }
            var genderToShow = new List<Gender>();
            if (filterParams.ShowFemale)
            {
                genderToShow.Add(Gender.Female);
            }
            if (filterParams.ShowMale)
            {
                genderToShow.Add(Gender.Male);
            }

            return r => genderToShow.Contains(r.Gender);
        }

        public ActionResult List(Sorting sort, FilterParams filterParams, int pageNumber = 1)
        {
            Func<Person, bool> specification = CreateSpecification(filterParams);

            var totalItemsCount = _personRepository.Persons.Where(specification).Count();
            var paging = new Paging
            {
                PageNumber = pageNumber,
                TotalItemsCount = totalItemsCount
            };
            var persons =
                _personRepository.Persons.Where(specification)
                .Skip((pageNumber - 1) * paging.PageSize).Take(paging.PageSize)
                .ToList();
            

            return Json(new PageOf<Person> { Data = persons, Paging = paging });
        }

        public ActionResult Test()
        {
            return View();
        }
    }

    public class PersonsViewModel
    {
        public FilterParams Filter { get; set; }
        public Sorting Sort { get; set; }
    }
}

