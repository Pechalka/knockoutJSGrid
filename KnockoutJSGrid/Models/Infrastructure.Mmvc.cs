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
    public class WindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer container;

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.Kernel.HasComponent(serviceType) ? container.ResolveAll(serviceType).Cast<object>() : new object[] { };
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