using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace RoadTrip.API.IOC
{
    public class IOCInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<AuthRepository>().ImplementedBy<AuthRepository>().LifestylePerWebRequest());
            container.Register(Component.For<AuthContext>().ImplementedBy<AuthContext>().LifestylePerWebRequest());
        }
    }
}