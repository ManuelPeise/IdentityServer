using Data.IdentityServerContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Web.IdentityServer.Helpers
{
    public class ServiceHelper
    {
        public static void ExecuteServiceHelper(IServiceCollection services, IConfiguration configuration)
        {
            var dbConnection = configuration.GetConnectionString("AuthContext");
            var migration = typeof(ServerContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ServerContext>(dbOptions =>
            {
                dbOptions.UseMySQL(configuration.GetConnectionString("AuthContext"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ServerContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddOperationalStore(options => options.ConfigureDbContext
                    = builder => builder.UseMySQL(dbConnection, sql => sql.MigrationsAssembly(migration)))
                .AddConfigurationStore(options => options.ConfigureDbContext
                    = builder => builder.UseMySQL(dbConnection, sql => sql.MigrationsAssembly(migration)))
                .AddDeveloperSigningCredential();

            

            services.AddCors(opt => opt.AddPolicy("Test", opt =>
            {
                opt.AllowAnyOrigin();
                opt.AllowAnyHeader();
                opt.AllowAnyMethod();

            }));
        }
    }
}
