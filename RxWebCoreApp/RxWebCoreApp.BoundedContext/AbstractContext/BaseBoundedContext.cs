using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RxWebCoreApp.Models;
using RxWebCoreApp.Models.Const;
using RxWebCoreApp.Models.Extensions;
using RxWebCoreApp.BoundedContext.Singleton;
using RxWeb.Core.Data;
using System;
using System.Linq;

namespace RxWebCoreApp.BoundedContext.SqlContext
{
    public abstract class BaseBoundedDbContext : DbContext
    {
        public BaseBoundedDbContext(BaseDbContext dbContext, DatabaseConfig databaseConfig, IHttpContextAccessor contextAccessor, TenantDbConnectionInfo tenantDbConnection)
        {
            DbContext = dbContext;
            TenantDbConnection = tenantDbConnection;
            DatabaseConfig = databaseConfig;
            ContextAccessor = contextAccessor;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(DbContext.Database.GetDbConnection(),
                options =>
                {
                    options.AddConnectionResiliency(this.DatabaseConfig.ConnectionResiliency);
                    options.CommandTimeout(this.DatabaseConfig.CommandTimeout);
                }).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddPropertyValueConversion();
            var tenantId = GetTenantId();
			var timeZoneName = GetTimeZoneName();
            if (tenantId != 0)
                modelBuilder.AddTenantFilter<int>(tenantId);
			if (!string.IsNullOrEmpty(timeZoneName))
                modelBuilder.AddTimeZoneValueConversion(timeZoneName);
			if (DatabaseConfig.MultiTenant.SchemaBasedMultiTenant)
            {
                var schemaInfo = TenantDbConnection.GetAsync(this.ContextAccessor.HttpContext.Request.Host.Value).Result;
                if (schemaInfo != null && schemaInfo.Keys.Contains(DbContext.Name))
                    modelBuilder.AddSchemaBasedTenant(schemaInfo[DbContext.Name]);
            }
        }

        private int GetTenantId()
        {
            var claim = this.ContextAccessor.HttpContext.User.Claims.Where(t => t.Type == CustomClaimTypes.TenantId).FirstOrDefault();
            return claim != null ? Convert.ToInt32(claim.Value) : 0;
        }

		private string GetTimeZoneName()
        {
            var claim = this.ContextAccessor.HttpContext.User.Claims.Where(t => t.Type == CustomClaimTypes.TimeZone).FirstOrDefault();
            return claim != null ? claim.Value : String.Empty;
        }

        private BaseDbContext DbContext { get; set; }

        private DatabaseConfig DatabaseConfig { get; set; }

        private IHttpContextAccessor ContextAccessor { get; set; }

        private TenantDbConnectionInfo TenantDbConnection { get; set; }
    }
}

