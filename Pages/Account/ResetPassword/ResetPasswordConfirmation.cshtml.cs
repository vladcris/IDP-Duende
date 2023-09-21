using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Duende.Pages.Account.ResetPassword
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        [BindProperty]
        public string ReturnUrl { get; set; }
        public IActionResult OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
