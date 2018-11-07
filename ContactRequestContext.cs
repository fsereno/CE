using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CE
{
    //Using EntityCore Framework
    // I think Generics could be useful here to paramaterise the collection type
    public class RequestContext<T> : DbContext where T : class
    {   
        public DbSet<T> Collection { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=OutContactDatabaseName.db");
        }
    }
}