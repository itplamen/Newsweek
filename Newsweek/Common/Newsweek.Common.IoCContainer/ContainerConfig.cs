namespace Newsweek.Common.IoCContainer
{
    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Common.IoCContainer.Packages;

    public static class ContainerConfig
    {
        public static void AddWorkerServices(this IServiceCollection services)
        {
            IPackage[] packages = new IPackage[]
            {
                new QueryHandlersPackage(),
                new CommandHandlersPackage(),
                new WorkerPackage()
            };

            RegisterServices(services, packages);
        }

        public static void AddWebServices(this IServiceCollection services)
        {
            IPackage[] packages = new IPackage[]
            {
                new QueryHandlersPackage(),
                new CommandHandlersPackage(),
                new WebPackage()
            };

            RegisterServices(services, packages);
        }

        private static void RegisterServices(IServiceCollection services, IPackage[] packages)
        {
            foreach (var package in packages)
            {
                package.RegisterServices(services);
            }
        }
    }
}