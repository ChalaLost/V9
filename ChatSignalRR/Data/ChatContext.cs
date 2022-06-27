using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ChatSignalRR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalRR.Data
{
    public class ChatContext : IdentityDbContext<ApplicationUser>
    {
        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(50).IsRequired(true);
            builder.Entity<ApplicationUser>().Property(x => x.Id).HasMaxLength(50).IsRequired(true);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }


        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
