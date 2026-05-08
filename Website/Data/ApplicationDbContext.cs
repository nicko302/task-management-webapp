using CRUD_Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("iRoles");
            builder.Entity<IdentityUserRole<string>>().ToTable("iUserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("iUserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("iUserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("iRoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("iUserTokens");
        }

        public DbSet<Board> Board { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<Models.Task> Task { get; set; }
        //public DbSet<User> User { get; set; }
        public DbSet<UserHasBoard> UserHasBoard { get; set; }

    }
}
