using Owin;

namespace Elders.Hystrix.NET.Dashboard
{
    public static class IAppBuilderExtensions
    {
        public static void UseHystrixDashboard(this IAppBuilder appBuilder)
        {
            var dashboard = new OwinConfig();
            dashboard.Configuration(appBuilder);
        }
    }
}
