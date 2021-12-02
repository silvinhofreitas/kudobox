using System;
using Kudobox.Helpers.Enums;
using Kudobox.Helpers.Extensions;
using Kudobox.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Kudobox.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            // TODO Remove this temporary admin before release

            User adminUser = new User
            {
                Id = Guid.Parse("3fd30650-4073-4f89-9bb8-0c3b1794aca3"),
                Username = "admin",
                Password = "admin".Encrypt(),
                Avatar = "https://ui-avatars.com/api/?name=Kudo+Box",
                DisplayName = "Kudobox Admin",
                Name = "Kudobox",
                Surname = "Admin",
                Email = "kudbox@kudobox.com",
                Status = UserStatusEnum.Active,
                Roles = "ADMIN;USER"
            };
            
            modelBuilder.Entity<User>()
                .HasData(adminUser);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}