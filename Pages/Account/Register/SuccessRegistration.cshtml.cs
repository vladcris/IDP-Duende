using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Duende.Pages.Account.Register
{
    [AllowAnonymous]
    public class SuccessRegistrationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
