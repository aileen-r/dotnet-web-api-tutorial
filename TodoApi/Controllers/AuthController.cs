using Microsoft.AspNetCore.Mvc;
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

    public AuthController(IUserService userService)
    {
      this.userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserInput userInput)
    {
      var user = await userService.Create(userInput);

      return Ok(user);
    }
  }
}
