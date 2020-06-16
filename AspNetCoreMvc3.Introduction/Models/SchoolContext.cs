using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMvc3.Introduction.Models
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
    }
}
