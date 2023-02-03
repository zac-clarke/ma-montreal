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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MeetingType> MeetingTypes { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }

        public MamDbContext(DbContextOptions<MamDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // modelBuilder.ApplyConfiguration(new MeetingConfiguration());

            builder.Entity<Meeting>()
            .HasOne(m => m.Gsr)
            .WithMany(au => au.MeetingsLead)
            .HasForeignKey("GsrId")
            // .OnDelete(DeleteBehavior.Restrict)
            ;

            builder.Entity<Meeting>()
            .HasOne(m => m.UpdatedBy)
            .WithMany(au => au.MeetingsUpdated)
            .HasForeignKey("UpdatedById")
            // .OnDelete(DeleteBehavior.Restrict)
            ;

            builder.Entity<UserRequest>()
            .HasOne(ur => ur.Requestee)
            .WithMany(au => au.UserRequestsSubmitted)
            .HasForeignKey("RequesteeId")
            // .OnDelete(DeleteBehavior.Restrict)
            ;

            builder.Entity<UserRequest>()
            .HasOne(ur => ur.RequestHandler)
            .WithMany(au => au.UserRequestsHandled)
            .HasForeignKey("RequestHandlerId")
            // .OnDelete(DeleteBehavior.Restrict)
            ;
            ;

            builder.Entity<Meeting>()
            .HasOne(m => m.Language)
            .WithMany(l => l.Meetings)
            // .OnDelete(DeleteBehavior.Restrict)
            ;

            builder.Entity<Meeting>()
            .HasOne(m => m.MeetingType)
            .WithMany(mt => mt.Meetings)
            // .OnDelete(DeleteBehavior.Restrict)
            ;
        }
    }
}