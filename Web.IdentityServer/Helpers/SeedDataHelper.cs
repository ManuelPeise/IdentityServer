using Data.IdentityServerContext;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.IdentityServer.Configuration;

namespace Web.IdentityServer.Helpers
{
    public class SeedDataHelper
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                foreach(var client in ServerConfiguration.GetClients())
                {
                    var existingClients = context.Clients.Where(c => c.ClientName.Equals(client.ClientName));

                    if (!existingClients.Any())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                }

                context.SaveChanges();

                foreach (var identityResource in ServerConfiguration.GetIdentityResources())
                {
                    var existingRecources = context.IdentityResources.Where(r => r.Name.Equals(identityResource.Name));

                    if (!existingRecources.Any())
                    {
                        context.IdentityResources.Add(identityResource.ToEntity());
                    }
                }

                context.SaveChanges();

                foreach(var apiScope in ServerConfiguration.GetApiScopes())
                {
                    var existingScopes = context.ApiScopes.Where(s => s.Name.Equals(apiScope.Name));

                    if (!existingScopes.Any())
                    {
                        context.ApiScopes.Add(apiScope.ToEntity());
                    }
                }

                context.SaveChanges();

                foreach (var res in ServerConfiguration.GetApiResources())
                {
                    var existingResources = context.ApiResources.Where(r => r.Name.Equals(res.Name));

                    if (!existingResources.Any())
                    {
                        context.ApiResources.Add(res.ToEntity());
                    }
                }

                context.SaveChanges();

                var userRoles = Enum.GetValues(typeof(UserRolesEnum));
                var serverContext = scope.ServiceProvider.GetRequiredService<ServerContext>();

                foreach(var role in userRoles)
                {
                    var existingRoles = serverContext.Roles.Where(r => r.Name.Equals(role.ToString())).ToList();

                    if (!existingRoles.Any())
                    {
                        serverContext.Roles.Add(new IdentityRole { Id = Guid.NewGuid().ToString(), NormalizedName = role.ToString(), Name = role.ToString() });
                    }
                }

                serverContext.SaveChanges();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var adminUsers = userManager.GetUsersInRoleAsync("Admin").Result;

                if (!adminUsers.Any())
                {
                    var testUser = new TestUser
                    {
                        SubjectId = Guid.NewGuid().ToString(),
                        Username = "Admin",
                        Password = "password",
                        Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Email, "admin@identityServer.com"),
                            new Claim(JwtClaimTypes.Role, "Admin")
                        }
                    };
                    
                    var user = new IdentityUser(testUser.Username)
                    {
                        Id = testUser.SubjectId
                        
                    };

                    var result = userManager.CreateAsync(user, "Password-1234").Result;

                    if (result.Succeeded)
                    {
                        result = userManager.AddToRoleAsync(user, "Admin").Result;

                        if (result.Succeeded)
                        {
                            userManager.AddClaimsAsync(user, testUser.Claims.ToList()).Wait();
                        }
                    }

                    serverContext.SaveChanges();
                }
            }
        }
    }
}
