using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.DataBase
{

    public class ZLDataBaseContext : DbContext
    {
        public ZLDataBaseContext()
        {
            
        }
        public ZLDataBaseContext(DbContextOptions<ZLDataBaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TbMenuRole>().HasKey(k => new { k.MenuId, k.RoleId });
            modelBuilder.Entity<TbUserRole>().HasKey(k => new { k.UserId, k.RoleId });
        }



        public DbSet<TbMenu> Menus { get; set; }
        public DbSet<TbRole> Roles { get; set; }
        public DbSet<TbUser> Users { get; set; }
        public DbSet<TbUserRole> UserRoles { get; set; }
        public DbSet<TbMenuRole> MenuRoles { get; set; }
        public DbSet<TbConfig> Configs { get; set; }
        public DbSet<TbDomain> Domains { get; set; }
        public DbSet<TbApplication> Applications { get; set; }
        public DbSet<TbStreamProxy> StreamProxies { get; set; }

        

    }
}