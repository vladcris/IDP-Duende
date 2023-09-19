
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Duende.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
}
