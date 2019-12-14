#region Namespace
using Microsoft.Extensions.DependencyInjection;
using RxWebCoreApp.Infrastructure.Security;
using RxWeb.Core.Data;
using RxWeb.Core.Security;
using RxWeb.Core.Annotations;
using RxWeb.Core;
using RxWebCoreApp.UnitOfWork.DbEntityAudit;
using RxWebCoreApp.BoundedContext.Main;
            using RxWebCoreApp.UnitOfWork.Main;
            #endregion Namespace



namespace RxWebCoreApp.Api.Bootstrap
{
    public static class ScopedExtension
    {

        public static void AddScopedService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepositoryProvider, RepositoryProvider>();
            serviceCollection.AddScoped<ITokenAuthorizer, TokenAuthorizer>();
            serviceCollection.AddScoped<ILocalizationInfo, LocalizationInfo>();
            serviceCollection.AddScoped<IModelValidation, ModelValidation>();
			serviceCollection.AddScoped<IAuditLog, AuditLog>();
			serviceCollection.AddScoped<IApplicationTokenProvider, ApplicationTokenProvider>();

            #region ContextService

                        serviceCollection.AddScoped<ILoginContext, LoginContext>();
            serviceCollection.AddScoped<ILoginUow, LoginUow>();
            #endregion ContextService



            #region DomainService
            
            #endregion DomainService
        }
    }
}




