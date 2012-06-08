using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
                Component.For<IPersonRepository>().ImplementedBy<PersonRepository>().
                DependsOn(Property.ForKey("databaseName").Eq("Persons")).
                DependsOn(Property.ForKey("collectionName").Eq("Persons"))
                );


            ControllerBuilder.Current.SetControllerFactory(new CastleControllerFactory(container));


            PersonsGenerator.MakeTestData();
        }
    }
}

