namespace TodoApi.Models.Users
{
  public class User
  {
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public HashSet<Role> Roles { get; set; } = new HashSet<Role>();

    #pragma warning disable CS8618 // Empty ctor for EF is not used
    public User() { }
    #pragma warning restore CS8618

    public User(string email, byte[] passwordHash, byte[] passwordSalt)
    {
      Id = Guid.NewGuid();
      Email = email;
      PasswordHash = passwordHash;
      PasswordSalt = passwordSalt;
      Roles = new HashSet<Role>()
      {
        Role.User
      };
    }
  }
}
