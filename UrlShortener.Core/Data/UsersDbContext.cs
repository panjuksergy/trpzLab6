using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SparkSwim.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkSwim.Core.Data
{
    public class UsersDbContext : IdentityDbContext<AppUser>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
           : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
