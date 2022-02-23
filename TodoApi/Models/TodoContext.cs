using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Users;
using TodoApi.Utilities;

namespace TodoApi.Models
{
  public class TodoContext : DbContext
  {
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
            .Property(e => e.Roles)
            .HasConversion(new EnumHashSetJsonValueConverter<Role>())
            .Metadata.SetValueComparer(new HashSetValueComparer<Role>());
    }
  }
}
