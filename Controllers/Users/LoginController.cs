using System.Linq;
using System.Threading.Tasks;
using AuthenticationProject.DTOs;
using AuthenticationProject.Extensions;
using AuthenticationProject.Models;
using AuthenticationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProject.Controllers;

[Route("v1/account")]
public class LoginController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("[controller]/login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] SignIn signIn)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(signIn.UserName, signIn.Password, false, false);

        if (!signInResult.Succeeded)
        {
            return new { signInResult };
        }

        var user = _userManager.Users.Where(x => x.UserName == signIn.UserName).FirstOrDefault();

        var userRoles = await _userManager.GetRolesAsync(user);

        var token = TokenService.GenerateToken(user, userRoles);
        signIn.Password = "";

        return new { token };
    }

    [HttpGet]
    [Route("[controller]/Anonymous")]
    [AllowAnonymous]
    public string Anonymous() => $"Hello, {User?.Identity?.Name ?? "Guest"}";

    [HttpGet]
    [Route("[controller]/Employee")]
    [Authorize(Roles = $"{UserRoles.Employee}, {UserRoles.Manager}")]
    public string Employee() => "Employee Authenticated";

    [HttpGet]
    [Route("[controller]/Manager")]
    [Authorize(Roles = UserRoles.Manager)]
    public string Manager() => "Manager Authenticated";

    [HttpGet]
    [Route("[controller]/Authenticated")]
    [Authorize]
    public string Authenticated() => string.Format("{0}, you are authenticated.", User.Identity.Name);
}
