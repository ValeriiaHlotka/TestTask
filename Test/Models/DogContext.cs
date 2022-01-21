using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Test.Models
{
    public class DogContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        public DogContext(DbContextOptions<DogContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
