using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Duende.Entities.InputModels;

public class TwoStepInputModel
{
    [Required]
    [DataType(DataType.Text)]
    public string TwoFactorCode { get; set; }
    public bool RememberLogin { get; set; }
}
