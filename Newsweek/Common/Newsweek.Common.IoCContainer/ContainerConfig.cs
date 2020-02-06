namespace Newsweek.Common.IoCContainer
{
    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Common.IoCContainer.Packages;

    public static class ContainerConfig
    {
        public static void AddServices(this IServiceCollection services)
        {
            IPackage[] packages = new IPackage[]
            {
                new QueryHandlersPackage(),
                new CommandHandlersPackage()
            };

            foreach (var package in packages)
            {
                package.RegisterServices(services);
            }
        }
    }
}