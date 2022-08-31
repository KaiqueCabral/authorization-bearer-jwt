using System.Collections.Generic;

namespace AuthenticationProject.DTOs;

public class RegisterUser
{
    public string FirsName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public IList<string> Roles { get; set; }
}
