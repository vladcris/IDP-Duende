using EmailService;
using IdentityProvider.Duende.Entities;
using IdentityProvider.Duende.Entities.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Duende.Pages.Account.ForgotPassword
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailSender emailSender;

        [BindProperty]
        public ForgotPasswordInputModel Input { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }

        public ForgotPasswordModel(UserManager<User> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }
        public IActionResult OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPost() {

            if(!ModelState.IsValid) {
                return Page();
            }

            var user = await userManager.FindByEmailAsync(Input.Email);
            if(user == null) {
                return RedirectToPage("/Account/ForgotPassword/ForgotPasswordConfirmation");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var callback = Url.Page("/Account/ResetPassword/ResetPassword", null, new {
                token,
                email = user.Email,
                ReturnUrl
            }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
            await emailSender.SendEmailAsync(message);

            return RedirectToPage("/Account/ForgotPassword/ForgotPasswordConfirmation");
        }

    }
}
