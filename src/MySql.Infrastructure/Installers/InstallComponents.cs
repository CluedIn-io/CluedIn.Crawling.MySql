using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;
using CluedIn.Crawling.MySql.Infrastructure.Factories;
using CluedIn.Core;

namespace CluedIn.Crawling.MySql.Infrastructure.Installers
{
    public class InstallComponents : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .AddFacilityIfNotExists<TypedFactoryFacility>()
                .Register(Component.For<IMySqlClientFactory>().AsFactory())
                .Register(Component.For<MySqlClient>().LifestyleTransient());
        }
    }
}
