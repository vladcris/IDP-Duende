using EmailService;
using IdentityProvider.Duende.Entities;
using IdentityProvider.Duende.Entities.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Duende.Pages.Account.Login
{
    [AllowAnonymous]
    public class LoginTwoStepModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<User> signInManager;

        [BindProperty]
        public TwoStepInputModel Input { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public LoginTwoStepModel(UserManager<User> userManager,
            IEmailSender emailSender,
            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
        }
        public  async Task<IActionResult> OnGet(string returnUrl, string email, bool rememberLogin)
        {
            Email = email;
            ReturnUrl = returnUrl;
            Input = new TwoStepInputModel {
                RememberLogin = rememberLogin
            };

            var user = await userManager.FindByEmailAsync(Email);
            if (user == null) {
                RedirectToPage("Account/Error", new { ReturnUrl = returnUrl });
            }

            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email")) {
                RedirectToPage("Account/Error", new { ReturnUrl = returnUrl });
            }

            var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email }, "Authentication token", token, null);

            await emailSender.SendEmailAsync(message);

            return Page();
        }

        public async Task<IActionResult> OnPost() {
            if(!ModelState.IsValid) {
                return Page();
            }

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if(user == null) {
                RedirectToPage("/Account/Error", new { ReturnUrl });
            }

            var result = await signInManager.TwoFactorSignInAsync("Email",
                Input.TwoFactorCode,
                Input.RememberLogin,
                rememberClient: false);

            if (result.Succeeded) {
                return this.LoadingPage(ReturnUrl);
            }
            else if (result.IsLockedOut) {
                await HandleLockout(Email, ReturnUrl);
                return Page();
            }
            else {
                RedirectToPage("/Account/Error", new { ReturnUrl });
            }

            return Page();

        }

        private async Task HandleLockout(string email, string returnUrl) {
            var user = await userManager.FindByEmailAsync(email);
            var forgotLink = Url.Page("/Account/ForgotPassword/ForgotPassword", null, new { returnUrl }, Request.Scheme);

            var content = @$"Your account is lockout, you can try again later or to reset your password,
                please click this link: {forgotLink}";

            var message = new Message(new string[] { user.Email }, "Account lockout", content, null);

            await emailSender.SendEmailAsync(message);

            ModelState.AddModelError("", "The account is locked out, check email for reset password");
        }
    }
}
