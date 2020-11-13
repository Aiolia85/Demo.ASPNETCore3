using Entity;
using Entity.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reoository.EF
{
    public class SQLiteDbContext:DbContext
    {
        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //   => options.UseSqlite("Data Source=sqlitedemo.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
        }

        public DbSet<UserInfo> Users { get; set; }
    }
}
