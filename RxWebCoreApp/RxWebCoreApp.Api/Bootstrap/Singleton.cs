using Microsoft.Extensions.DependencyInjection;
using RxWebCoreApp.Infrastructure.Singleton;
using RxWebCoreApp.BoundedContext.Singleton;

namespace RxWebCoreApp.Api.Bootstrap
{
    public static class Singleton
    {
        public static void AddSingletonService(this IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton(typeof(TenantDbConnectionInfo));
            serviceCollection.AddSingleton(typeof(UserAccessConfigInfo));
        }

    }
}




