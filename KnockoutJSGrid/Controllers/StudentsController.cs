using System.Collections.Generic;
using System.Web.Mvc;
using KnockoutJSGrid.Models;

namespace KnockoutJSGrid.Controllers
{
    public class StudentsController : Controller
    {
        public ActionResult Index()
        {
            var filter = new StudentFilter();
            filter.Conditions = new List<KeyValuePair<string, string>>
                                    {
                                        new KeyValuePair<string, string>("", ""),
                                        new KeyValuePair<string, string>("", ""),
                                        new KeyValuePair<string, string>("", ""),
                                        new KeyValuePair<string, string>("", "")
                                    };

            var viewModel = new StudentViewModel
                                {
                                    Sort = new Sorting
                                               {
                                                   Field = "Name",
                                                   Distinct = "asc"
                                               },
                                               Filter = filter
                                };
            return View(viewModel);
        }

        public JsonResult UpdateGrid(int pageNumber = 1)
        {
            var data = new PageOf<Student>
                           {
                               Data = new List<Student>
                                          {
                                              new Student{},
                                              new Student{},
                                              new Student{},
                                              new Student{}
                                          },
                                          Paging = new Paging
                                                       {
                                                           PageNumber = pageNumber,
                                                           TotalItemsCount = 4
                                                       }
                           };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
