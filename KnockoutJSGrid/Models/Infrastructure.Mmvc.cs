using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace KnockoutJSGrid.Models
{
    public class WindsorDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }
    }


    public class CastleControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;

        public CastleControllerFactory(IWindsorContainer container)
        {
            _container = container;
            _container.Register(
                AllTypes.FromThisAssembly()
                    .BasedOn<IController>()
                    .Configure(c => c.LifestyleTransient()));
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                return (IController)_container.Resolve(controllerType);
            }
            else
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
        }
    }
}