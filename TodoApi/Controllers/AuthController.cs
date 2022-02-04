using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Dtos.Users;
using TodoApi.Models.Users;
using TodoApi.Services;

namespace TodoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IUserService userService;
    private readonly IAuthService authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
      this.userService = userService;
      this.authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRegisterInput input)
    {
      var user = await userService.Create(input);

      return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserInput input)
    {
      var userExists = await userService.CheckIfUserExists(input.Email);

      // TODO: for prod these should return the same error
      if (!userExists)
      {
        return BadRequest("User not found.");
      }

      var correctPassword = await userService.VerifyUserPassword(input);
      if (!correctPassword)
      {
        return BadRequest("Incorrect password.");
      }

      string token = authService.CreateToken(input.Email);
      return Ok(token);
    }
  }
}
