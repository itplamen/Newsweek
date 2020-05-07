namespace Newsweek.Tests.Common.Mapping
{
    using System.Reflection;

    using Newsweek.Common.Constants;
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Web.ViewModels.Common;

    public static class MappingConfig
    {
        private static object lockObject = new object();

        public static void Initialize()
        {
            lock (lockObject)
            {
                AutoMapperConfig.RegisterMappings(
                       typeof(ErrorViewModel).GetTypeInfo().Assembly,
                       Assembly.Load(PublicConstants.COMMANDS_ASSEMBLY));
            }
        }
    }
}