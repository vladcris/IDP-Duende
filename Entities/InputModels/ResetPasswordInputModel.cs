using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Duende.Entities.InputModels;

public class ResetPasswordInputModel
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
