using LiveNotificationsMVC.SubscribeTableDependencies;
using System.Runtime.CompilerServices;

namespace LiveNotificationsMVC.MiddlewareExtensions;

public static class ApplicationBuilderExtensions
{
    public static void UseSqlTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString)
        where T : ISubscribeTableDependency
    {
        var serviceProvider = applicationBuilder.ApplicationServices;
        var service = serviceProvider.GetService<T>();
        service?.CheckForDependencies(connectionString);
    }
}
