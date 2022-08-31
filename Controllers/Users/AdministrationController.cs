using System.Threading.Tasks;
using AuthenticationProject.DTOs;
using AuthenticationProject.Extensions;
using AuthenticationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProject.Controllers.Users;

[Route("v1/account")]
public class AdministrationController : Controller
{
    private readonly UserManager<User> _userManager;

    public AdministrationController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    [Route("[controller]/users/register")]
    [Authorize(Roles = $"{UserRoles.Admin}")]
    public async Task<ActionResult<dynamic>> Register([FromBody] RegisterUser registerUser)
    {
        try
        {
            var user = new User()
            {
                Email = registerUser.Email,
                NormalizedEmail = registerUser.Email,
                UserName = registerUser.UserName,
                NormalizedUserName = registerUser.UserName,
                FirsName = registerUser.FirsName,
                LastName = registerUser.LastName,
                PasswordHash = registerUser.PasswordHash,
                PhoneNumber = registerUser.PhoneNumber,
                TwoFactorEnabled = registerUser.TwoFactorEnabled
            };

            var createUser = await _userManager.CreateAsync(user, user.PasswordHash);

            if (createUser.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, registerUser.Roles);
            }

            return new { createUser };
        }
        catch
        {
            throw;
        }
    }
}
