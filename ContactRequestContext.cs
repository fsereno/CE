using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace CE
{
    //Attempting to use EntityCore Framework
    // I think Generics could be useful here to paramaterise the collection type
    public class RequestContext<T> : DbContext where T : class
    {   
        public DbSet<T> Entities { get; set; }
        // Below is taken from ms docs at https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite
        // I studied these docs before working on solution
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=OurContactDatabaseName.db");
        }
    }
}