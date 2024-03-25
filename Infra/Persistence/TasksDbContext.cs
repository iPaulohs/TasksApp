using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence
{
    public class TasksDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public new DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<ListCard> Lists { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
