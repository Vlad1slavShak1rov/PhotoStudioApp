using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PhotoStudioApp.Model;
namespace PhotoStudioApp.Database.DBContext
{
    public class MyDBContext : DbContext
    {
        private readonly string connectionString = "Server=DESKTOP-N2FEHOR\\MSSQLSERVER01; Database=PhotoStudio; Trusted_Connection=True; TrustServerCertificate=True;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(connectionString);

        public DbSet<AdditionalService> AdditionalServices { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<HistoryPointsReceived> HistoryPoints { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
               .HasOne(b => b.Photograph)
               .WithMany()
               .HasForeignKey(b => b.PhotographID)
               .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Visagiste)
                .WithMany()
                .HasForeignKey(b => b.VisagisteID)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Visagiste)
                .WithMany()
                .HasForeignKey(b => b.VisagisteID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hall)
                .WithMany(h => h.Bookings)
                .HasForeignKey(b => b.HallID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Services)
                .WithMany()
                .HasForeignKey(b => b.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.AdditionalService)
                .WithMany()
                .HasForeignKey(b => b.AdditionalServicesID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

        

            modelBuilder.Entity<Worker>()
                .HasOne(w => w.User)
                .WithOne(u => u.Worker)
                .HasForeignKey<Worker>(w => w.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithOne(r => r.Review)
                .HasForeignKey<Review>(r => r.BookingID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
