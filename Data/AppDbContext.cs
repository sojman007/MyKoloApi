using Microsoft.EntityFrameworkCore;
using MyKoloApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyKoloApi.Data
{
    public class AppDbContext:DbContext
    {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<UserClaim> Claims { get; set; }

        public AppDbContext(DbContextOptions options):base(options)
        {



        }

    }
}
