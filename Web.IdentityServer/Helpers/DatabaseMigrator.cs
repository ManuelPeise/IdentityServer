using Data.IdentityServerContext;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.IdentityServer.Helpers
{
    public class DatabaseMigrator
    {
        public static void ExecuteMigrations(IApplicationBuilder builder)
        {
            MigratePersistGrandContext(builder);
            MigrateConfigurationContext(builder);
            MigrateSserverContext(builder);
        }

        private static void MigrateSserverContext(IApplicationBuilder builder)
        {
            var serviceScopeFactory = (IServiceScopeFactory)builder.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var serverContext = services.GetRequiredService<ServerContext>();

                if (serverContext.Database.GetPendingMigrations().Any())
                {
                    serverContext.Database.Migrate();
                }

            }
        }

        private static void MigrateConfigurationContext(IApplicationBuilder builder)
        {
            var serviceScopeFactory = (IServiceScopeFactory)builder.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configurationContext = services.GetRequiredService<ConfigurationDbContext>();

                if (configurationContext.Database.GetPendingMigrations().Any())
                {
                    configurationContext.Database.Migrate();
                }

            }
        }

        private static void MigratePersistGrandContext(IApplicationBuilder builder)
        {
            var serviceScopeFactory = (IServiceScopeFactory)builder.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var persistentContext = services.GetRequiredService<PersistedGrantDbContext>();

                if (persistentContext.Database.GetPendingMigrations().Any())
                {
                    persistentContext.Database.Migrate();
                }

            }
        }
    }
}
