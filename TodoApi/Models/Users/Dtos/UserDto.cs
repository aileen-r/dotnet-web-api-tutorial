
using TodoApi.Models;

namespace TodoApi.Models.Users.Dtos
{
  public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public HashSet<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
