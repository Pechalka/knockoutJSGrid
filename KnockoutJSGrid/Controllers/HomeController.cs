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
        public ActionResult Index()
        {
            return View(defaultPersonsViewModel());
        }

        private PersonsViewModel defaultPersonsViewModel()
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

 
        public ActionResult List(Sorting sort, FilterParams filterParams, int pageNumber = 1)
        {
            var query = For<Person>().Set(sort).FindBy(filterParams).Page(pageNumber).Limite(10);

            var onePageOfPersons = query.Execute();
            

            return Json(onePageOfPersons);
        }

        private IQuery<T> For<T>()
        {
            return DependencyResolver.Current.GetService<IQuery<T>>();
        }
    }
}
