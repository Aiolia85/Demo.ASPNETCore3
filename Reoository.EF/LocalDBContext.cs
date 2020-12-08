using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reoository.EF
{
    public class TestTable
    { 
        public int Id { get; set; }
        public string StrCol { get; set; }
    }
    public class LocalDBContext : DbContext
    {
        public LocalDBContext(DbContextOptions<LocalDBContext> options) : base(options)
        {
        }


        public DbSet<TestTable> TestTable { get; set; }
    }

    public class LocalDBContext1 : DbContext
    {
        public LocalDBContext1(DbContextOptions<LocalDBContext1> options) : base(options)
        {
        }


        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
