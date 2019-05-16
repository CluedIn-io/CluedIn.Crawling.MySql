using Castle.Windsor;
using CluedIn.Core;
using CluedIn.Core.Providers;
using CluedIn.Crawling.MySql.Infrastructure.Factories;
using Moq;

namespace Provider.MySql.Test.MySqlProvider
{
    public abstract class MySqlProviderTest
    {
        protected readonly ProviderBase Sut;

        protected Mock<IMySqlClientFactory> NameClientFactory;
        protected Mock<IWindsorContainer> Container;

        protected MySqlProviderTest()
        {
            Container = new Mock<IWindsorContainer>();
            NameClientFactory = new Mock<IMySqlClientFactory>();
            var applicationContext = new ApplicationContext(Container.Object);
            Sut = new CluedIn.Provider.MySql.MySqlProvider(applicationContext, NameClientFactory.Object);
        }
    }
}
