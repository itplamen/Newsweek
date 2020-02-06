namespace Newsweek.Common.IoCContainer.Packages
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IPackage
    {
        void RegisterServices(IServiceCollection services);
    }
}