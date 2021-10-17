using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.IdentityServerContext
{
    public class ServerContext: IdentityDbContext
    {
        public ServerContext(DbContextOptions<ServerContext> options) : base(options) { }
    }
}
