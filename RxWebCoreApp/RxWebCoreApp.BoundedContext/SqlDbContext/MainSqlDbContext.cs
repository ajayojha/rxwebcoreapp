using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RxWebCoreApp.BoundedContext.Singleton;
using RxWebCoreApp.Models;
using System;

namespace RxWebCoreApp.BoundedContext.SqlContext
{
    public class MainSqlDbContext : BaseDbContext, IMainDatabaseFacade, IDisposable
    {
        public MainSqlDbContext(IOptions<DatabaseConfig> databaseConfig, IHttpContextAccessor contextAccessor, TenantDbConnectionInfo tenantDbConnection) :base(databaseConfig, contextAccessor,tenantDbConnection){}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.GetConnection("Main"));
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}
