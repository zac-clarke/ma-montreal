using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Data
{
    public class MamDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingType> MeetingTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }

        public MamDbContext(DbContextOptions<MamDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // modelBuilder.ApplyConfiguration(new MeetingConfiguration());

            // builder.Entity<Meeting>()
            // .HasOne(m => m.Gsr)
            // .WithMany(u => u.MeetingsLead)
            // .HasForeignKey(m => m.Gsr);

            // builder.Entity<Meeting>()
            // .HasOne(m => m.Gsr)
            // .WithMany(navigationExpression: u => u.MeetingsUpdated)
            // .HasForeignKey(m => m.Gsr);
        }
    }
}