using IdentityProvider.Duende.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Identity;
using EmailService;

namespace IdentityProvider.Duende;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddAutoMapper(typeof(Program));

        var config = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        builder.Services.AddSingleton(config);
        builder.Services.AddScoped<IEmailSender, EmailSender>();

        var migrationAssembly = typeof(Program).Assembly.GetName().Name;

        builder.Services.AddDbContext<UserContext>(opt => {
            opt.UseSqlite(builder.Configuration.GetConnectionString("identitySqlite"));
        });

        builder.Services.AddIdentity<User, IdentityRole>(opt => {
            
        })
            .AddEntityFrameworkStores<IdentityProvider.Duende.Entities.UserContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => {
            opt.TokenLifespan = TimeSpan.FromHours(2);
        });

        builder.Services.AddIdentityServer(options => {
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
            options.EmitStaticAudienceClaim = true;
        })
            //.AddInMemoryIdentityResources(Config.IdentityResources)
            //.AddInMemoryApiScopes(Config.ApiScopes)
            //.AddInMemoryApiResources(Config.Apis)
            //.AddInMemoryClients(Config.Clients)
            //.AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(opt => {
                opt.ConfigureDbContext = c => c.UseSqlite(builder.Configuration.GetConnectionString("sqlite"),
                    opt => opt.MigrationsAssembly(migrationAssembly));
            }).AddOperationalStore(opt => {
                opt.ConfigureDbContext = c => {
                    c.UseSqlite(builder.Configuration.GetConnectionString("sqlite"),
                        opt => opt.MigrationsAssembly(migrationAssembly));
                };
            })
            .AddAspNetIdentity<User>();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
