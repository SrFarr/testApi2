﻿
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;
namespace UserAPI.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pastikan nama tabel sesuai di database
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
