using Microsoft.AspNetCore.Identity;

namespace AuthenticationProject.Models;

public class User : IdentityUser
{
    public string FirsName { get; set; }
    public string LastName { get; set; }
}
