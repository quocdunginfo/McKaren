using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using McKaren.Db.Models;

namespace McKaren.Db
{
    public class McKarenDb : DbContext
    {
        public McKarenDb()
            : base("name=McKarenDb")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasMany<Role>(s => s.Roles)
                        .WithMany(c => c.Users)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("User");
                            cs.MapRightKey("Role");
                            cs.ToTable("McKaren.UserRoleMapping");
                        });
            modelBuilder.Entity<Role>()
                        .HasMany<Permission>(s => s.Permissions)
                        .WithMany(c => c.Roles)
                        .Map(cs =>
                        {
                            cs.MapLeftKey("Role");
                            cs.MapRightKey("Permission");
                            cs.ToTable("McKaren.RolePermissionMapping");
                        });
        }
    }
}