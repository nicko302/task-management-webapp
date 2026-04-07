using CRUD_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Application.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Board> Board { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<Models.Task> Task { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserHasBoard> UserHasBoard { get; set; }

    }
}
