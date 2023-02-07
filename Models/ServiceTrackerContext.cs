using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ServiceTracker.Models
{
    public class ServiceTrackerContext : DbContext
    {
        public ServiceTrackerContext (DbContextOptions<ServiceTrackerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ServiceTracker.Models.Employee> Employees { get; set; }
        public virtual DbSet<Committees> Committees { get; set; }
        public virtual DbSet<CommitteeMembers> CommitteeMembers { get; set; }
        public virtual DbSet<Members> Members { get; set; }

        public static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }
          
    


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ucpath_id");

                entity.Property(e => e.FirstName).HasColumnName("first_name");
                entity.Property(e => e.LastName).HasColumnName("last_name");
                entity.Property(e => e.VoteCategory).HasColumnName("vote_category");
                entity.Property(e => e.AdminStaff).HasColumnName("admin_staff");

            });
        }
    }
}