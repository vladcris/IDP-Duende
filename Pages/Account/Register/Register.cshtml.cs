using AutoMapper;
using IdentityModel;
using IdentityProvider.Duende.Entities;
using IdentityProvider.Duende.Entities.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IdentityProvider.Duende.Pages.Account.Register
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        [BindProperty]
        public UserRegistrationModel Input { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }

        public RegisterModel(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
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

            var user = mapper.Map<User>(Input);

            var result = await userManager.CreateAsync(user, Input.Password);
            
            if(!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return Page();
            }

            await userManager.AddToRoleAsync(user, "Visitor");

            await userManager.AddClaimsAsync(user, new List<Claim> {
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Role, "Visitor"),
                new Claim(JwtClaimTypes.Address, user.Address),
                new Claim("country", user.Country)
            });

            return Redirect(ReturnUrl);
        }
    }
}
