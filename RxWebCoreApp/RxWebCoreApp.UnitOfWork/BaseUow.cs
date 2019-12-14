using RxWeb.Core.Data;

namespace RxWebCoreApp.UnitOfWork
{
    public class BaseUow : CoreUnitOfWork
    {
        public BaseUow(IDbContext context, IRepositoryProvider repositoryProvider,IAuditLog auditLog = null) : base(context, repositoryProvider,auditLog) { }
    }
}


