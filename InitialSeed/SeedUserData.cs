using IdentityModel;
using IdentityProvider.Duende.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityProvider.Duende.InitialSeed;

public class SeedUserData
{
    public static void EnsureSeedData(string connectionString) {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddDbContext<UserContext>(options =>
                   options.UseSqlite(connectionString));

        services.AddIdentity<User, IdentityRole>(opt => {
            opt.Password.RequireDigit = false;
            opt.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        CreateUser(scope, "John", "Doe", "John Doe's Boulevard 323", "USA",
                 "97a3aa4a-7a89-47f3-9814-74497fb92ccb", "JohnPassword",
                 "Admin", "john@mail.com");

        CreateUser(scope, "Jane", "Doe", "Jane Doe's Avenue 214", "USA",
                 "64aca900-7bc7-4645-b291-38f1b7b5963c", "JanePassword",
                 "Visitor", "jane@mail.com");    }

    public static void CreateUser(IServiceScope provider, string name, string LastName,
        string address, string country, string id, string password, string role, string email) {
        
        var userManager = provider.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = userManager.FindByNameAsync(email).Result;
        if (user == null) {
            user = new User {
                UserName = email,
                Email = email,
                FirstName = name,
                LastName = LastName,
                Address = address,
                Country = country,
                Id = id
            };
        }
        var result = userManager.CreateAsync(user, password).Result;
        CheckResult(result);

        result = userManager.AddToRoleAsync(user, role).Result;
        CheckResult(result);

        result = userManager.AddClaimsAsync(user, new Claim[] {
            new Claim(JwtClaimTypes.GivenName, user.FirstName),
            new Claim(JwtClaimTypes.FamilyName, user.LastName),
            new Claim(JwtClaimTypes.Role, role),
            new Claim(JwtClaimTypes.Address, user.Address),
            new Claim("country", user.Country),
        }).Result;
        CheckResult(result);
    }

    private static void CheckResult(IdentityResult result) {
        if (!result.Succeeded) {
            throw new Exception(result.Errors.First().Description);
        }
    }
}
