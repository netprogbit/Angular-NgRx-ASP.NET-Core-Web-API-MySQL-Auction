using DataLayer.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Contexts
{
    public class AccountDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // User initialization

            modelBuilder.Entity<User>().HasData(
               new User[]
               {
                    new User 
                    { 
                        Id = "de6b3e90-39f9-45cb-a96e-11a5ef35f4d4", 
                        UserName = "Administrator", 
                        Email = "admin@auction.com",                         
                        PasswordHash = "CQF9pVh87cIuoNg0xksMsOEJrcqD86hy/H9P8fSjl8mk5ymCjBE2ZOrm1l0C6DlV5xhVeX7I9ecQ8upjo7/Dcg==" 
                    },
                    new User
                    {
                        Id = "bfcd425d-360c-4619-b1dc-ca62099e4732",
                        UserName = "Ivan",
                        Email = "ivan@gmail.com", 
                        PasswordHash = "WgkOqhMuNVfIVBZxP++JdOWBXZkVWNbztiLBuV2ICeZxV1aDC3Rl3DHaTaDqzKdaqy0LQio+kJdy6xxDFlNR3Q=="
                    },
                    new User
                    {
                        Id = "70ab65b3-882c-4807-94d0-541c7bf75b6f",
                        UserName = "Valya",
                        Email = "valya@gmail.com",
                        PasswordHash = "AuJLHlx9Dmj1WQvvNgWJKnwRilG7+cqsTWsKuOsQi/O7Wd16hjbGIJp0dq4oWcQZlzaPTYe2n9Jwq7p/hTTwmA=="
                    },
               });

            modelBuilder.Entity<Role>().HasData(
               new Role[]
               {
                    new Role { Id = "6def4e6d-c93d-4a3e-b55a-03a5fd6644a1", Name = "User", NormalizedName = "USER" },
                    new Role { Id = "517d32da-b4ea-4dee-a559-3105719d6dbb", Name = "Admin", NormalizedName = "ADMIN" },
               });

            modelBuilder.Entity<UserRole>().HasData(
               new UserRole[]
               {
                   new UserRole { UserId = "de6b3e90-39f9-45cb-a96e-11a5ef35f4d4", RoleId = "6def4e6d-c93d-4a3e-b55a-03a5fd6644a1" },
                   new UserRole { UserId = "de6b3e90-39f9-45cb-a96e-11a5ef35f4d4", RoleId = "517d32da-b4ea-4dee-a559-3105719d6dbb" },
                   new UserRole { UserId = "bfcd425d-360c-4619-b1dc-ca62099e4732", RoleId = "6def4e6d-c93d-4a3e-b55a-03a5fd6644a1" },
                   new UserRole { UserId = "70ab65b3-882c-4807-94d0-541c7bf75b6f", RoleId = "6def4e6d-c93d-4a3e-b55a-03a5fd6644a1" },
               });
        }
    }
}
