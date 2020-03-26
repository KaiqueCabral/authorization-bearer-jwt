using AuthenticationProject.Models;
using AuthenticationProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationProject.Controllers
{
    [Route("v1/account")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            User user = await UserRepository.Get(model.Username, model.Password);

            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos!" });
            }

            string token = TokenService.GenerateToken(user);
            user.Password = "";
            return new { user, token };
        }

        [HttpGet]
        [Route("Anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anonymous";

        [HttpGet]
        [Route("Employee")]
        [Authorize(Roles = "Employee,Manager")]
        public string Employee() => "Employee Authenticated";

        [HttpGet]
        [Route("Manager")]
        [Authorize(Roles = "Manager")]
        public string Manager() => "Manager Authenticated";

        [HttpGet]
        [Route("Authenticated")]
        [Authorize]
        public string Authenticated() => string.Format("{0}, you are authenticated.", User.Identity.Name);
    }
}
