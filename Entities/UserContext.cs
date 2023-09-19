using IdentityProvider.Duende.Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Duende.Entities;

public class UserContext : IdentityDbContext<User>
{
    public UserContext(DbContextOptions<UserContext> opt) : base(opt)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        //builder.ApplyConfiguration(new RoleConfiguration());
        base.OnModelCreating(builder);
    }
}
