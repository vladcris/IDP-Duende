using IdentityProvider.Duende.Entities;
using IdentityProvider.Duende.Entities.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Duende.Pages.Account.Register
{
    [AllowAnonymous]
    public class ConfirmMailModel : PageModel
    {
        private readonly UserManager<User> userManager;
        [BindProperty]
        public ConfirmMailInputModel Input { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }
        public ConfirmMailModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string returnUrl, string token, string email)
        {
            ReturnUrl = returnUrl;
            Input = new ConfirmMailInputModel { Email = email, Token = token };

            var user = await userManager.FindByEmailAsync(Input.Email);
            if (user == null) {
                return RedirectToPage("/Account/Error", new {ReturnUrl});
            }

            var result = await userManager.ConfirmEmailAsync(user, Input.Token);
            if (result.Succeeded) {
                return Page();
            }

            return RedirectToPage("/Account/Error");
        }
    }
}
