using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Duende.Entities.InputModels;

public class ForgotPasswordInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
