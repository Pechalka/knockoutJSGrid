using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using KnockoutJSGrid.Controllers;
using KnockoutJSGrid.Models;

namespace KnockoutJSGrid
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();

            container.Register(
                Component.For<IQuery<IQueryable<Person>, FilterParams>>().ImplementedBy<FindPersonsQuery>(),
                Component.For<IDefaultValueFor<PersonsViewModel>>().ImplementedBy<DefaultValueStorage>()

                );

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
            ControllerBuilder.Current.SetControllerFactory(new CastleControllerFactory(container));
            
            ModelBinders.Binders.DefaultBinder = new DIModelBinder();

            PersonsGenerator.MakeTestData();

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
    }


}

