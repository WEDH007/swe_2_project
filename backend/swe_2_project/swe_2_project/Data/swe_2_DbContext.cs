using System;
using Microsoft.EntityFrameworkCore;
using swe_2_project.Models;

namespace swe_2_project.Data
{
	public class swe_2_DbContext : DbContext
	{
		public swe_2_DbContext(DbContextOptions<swe_2_DbContext> options) : base (options) {}
		public DbSet<users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<users>().Property(e => e.status).HasConversion(v => v.ToString(), v => (user_status)Enum.Parse(typeof(user_status), v));
        }
    }
}

