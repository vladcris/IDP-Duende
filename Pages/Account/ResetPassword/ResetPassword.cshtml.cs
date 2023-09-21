using IdentityProvider.Duende.Entities;
using IdentityProvider.Duende.Entities.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Duende.Pages.Account.ResetPassword
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public ResetPasswordInputModel Input { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }

        public ResetPasswordModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult OnGet(string returnUrl, string email, string token)
        {
            ReturnUrl = returnUrl;
            Input = new ResetPasswordInputModel { Email = email, Token = token };

            return Page();
        }

        public async Task<IActionResult> OnPost() {
            if(!ModelState.IsValid) {
                return Page();
            }

            var user = await userManager.FindByEmailAsync(Input.Email);
            if(user == null) {
                return RedirectToPage("/Account/ResetPassword/ResetPasswordConfirmation", new { ReturnUrl });
            }

            var resetPassword = await userManager.ResetPasswordAsync(user, Input.Token, Input.Password);
            if(!resetPassword.Succeeded) {
                foreach(var error in resetPassword.Errors) {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return Page();
            }
            if (await userManager.IsLockedOutAsync(user)) {
                await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(new
               DateTime(1000, 1, 1, 1, 1, 1)));
            }
            return RedirectToPage("/Account/ResetPassword/ResetPasswordConfirmation", new { ReturnUrl });
        }
    }
}
