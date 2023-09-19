using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duende.IdentityServer.Hosting;

namespace IdentityProvider.Duende.Pages.Home;

[AllowAnonymous]
public class Index : PageModel
{
    public string Version;
        
    public void OnGet()
    {
        Version = typeof(IdentityServerMiddleware).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+').First();
    }
}