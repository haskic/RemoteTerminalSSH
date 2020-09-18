using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZModels;

namespace BackEnd.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) :
            base(options)
        {
        }
        public DbSet<Terminal> Desktops { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

    }
}
