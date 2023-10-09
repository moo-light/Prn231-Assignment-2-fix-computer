using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Domain
{
    public class AppDBContext : DbContext
    {
        public AppDBContext() : this(new())
        {

        }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CarRental> CarRentals { get; set; }
        public DbSet<CarProducer> Producers { get; set; }


        public static string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration.GetConnectionString("DefaultDB") ?? "";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.LogTo(Console.WriteLine, LogLevel.Information).UseSqlServer(GetConnectionString());
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // debug stuff goes here
                option.EnableSensitiveDataLogging();
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            }
            else
            {
                // release stuff goes here
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Car>(opt =>
            {
                opt.Property(x => x.Status).HasDefaultValue(true).IsRequired();
                opt.Property(x => x.ImportDate).HasDefaultValueSql("getdate()");
            });
            builder.Entity<Customer>(opt =>
            {
                opt.Property(x => x.Password).IsRequired();
                opt.HasIndex(x => x.Email).IsUnique();
                opt.HasIndex(x => x.Mobile).IsUnique();
                opt.HasIndex(x => x.IdentityCard).IsUnique();
                opt.HasIndex(x => x.LicenceNumber).IsUnique();
            });
            builder.Entity<CarRental>(opt =>
            {
                opt.Property(x => x.Status).HasDefaultValue(true).IsRequired();
                opt.Property(x => x.PickupDate).HasColumnType("date");
                opt.Property(x => x.ReturnDate).HasColumnType("date");
                opt.HasKey(x => new { x.CarId, x.CustomerId, x.PickupDate });
            });
            builder.Entity<Review>(opt =>
            {
                opt.HasKey(x => new { x.CarId, x.CustomerId });
            });
            base.OnModelCreating(builder);
        }
    }
}
