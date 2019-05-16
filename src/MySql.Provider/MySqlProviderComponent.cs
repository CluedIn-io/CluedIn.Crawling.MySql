using Castle.MicroKernel.Registration;

using CluedIn.Core;
using CluedIn.Core.Providers;
using CluedIn.Core.Webhooks;
using CluedIn.Crawling.MySql.Core;
using CluedIn.Crawling.MySql.Infrastructure.Installers;
using CluedIn.Server;

using ComponentHost;

namespace CluedIn.Provider.MySql
{
    [Component(MySqlConstants.ProviderName, "Providers", ComponentType.Service, ServerComponents.ProviderWebApi, Components.Server, Components.DataStores, Isolation = ComponentIsolation.NotIsolated)]
    public class MySqlProviderComponent : ServiceApplicationComponent<EmbeddedServer>
    {
        /**********************************************************************************************************
         * CONSTRUCTOR
         **********************************************************************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlProviderComponent" /> class.
        /// </summary>
        /// <param name="componentInfo">The component information.</param>
        public MySqlProviderComponent(ComponentInfo componentInfo)
            : base(componentInfo)
        {
            Container.Register(Component.For<MySqlProviderComponent>().Instance(this));  // Dev. Note: bad practice to call virtual member in ctor
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        /// <summary>Starts this instance.</summary>
        public override void Start()
        {
            Container.Install(new InstallComponents());
            
            Container.Register(Types.FromThisAssembly().BasedOn<IProvider>().WithServiceFromInterface().If(t => !t.IsAbstract).LifestyleSingleton());
            Container.Register(Types.FromThisAssembly().BasedOn<IEntityActionBuilder>().WithServiceFromInterface().If(t => !t.IsAbstract).LifestyleSingleton());

            State = ServiceState.Started;
        }

        /// <summary>Stops this instance.</summary>
        public override void Stop()
        {
            if (State == ServiceState.Stopped)
                return;

            State = ServiceState.Stopped;
        }
    }
}
