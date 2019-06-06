using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.MySql.Factories;

namespace CluedIn.Crawling.MySql.Infrastructure.Installers
{
    public class InstallComponents : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container
            //    .Register(Component.For<IClueFactory>().ImplementedBy<MySqlClueFactory>());
        }
    }
}
