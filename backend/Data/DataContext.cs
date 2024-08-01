using HotDeskBookingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingStatus> BookingStatuses { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<OfficeFloor> Floors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // UserRoles

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserEmail, ur.RoleName });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserEmail)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleName)
                .OnDelete(DeleteBehavior.Restrict);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Roles

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleName);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleName)
                .OnDelete(DeleteBehavior.Cascade);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // AppUsers

            modelBuilder.Entity<AppUser>()
                .HasKey(u => u.Email);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserEmail)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserEmail)
                .OnDelete(DeleteBehavior.Cascade);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // BookingStatuses

            modelBuilder.Entity<BookingStatus>()
                .HasKey(bs => bs.StatusName);

            modelBuilder.Entity<BookingStatus>()
                .HasMany(bs => bs.Bookings)
                .WithOne(b => b.BookingStatus)
                .HasForeignKey(b => b.BookingStatusName)
                .OnDelete(DeleteBehavior.SetNull);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Bookings

            modelBuilder.Entity<Booking>()
                .HasKey(b => b.BookingId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserEmail)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Desk)
                .WithMany(d => d.Bookings)
                .HasForeignKey(b => b.DeskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.BookingStatus)
                .WithMany(bs => bs.Bookings)
                .HasForeignKey(b => b.BookingStatusName)
                .OnDelete(DeleteBehavior.Restrict);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Desks

            modelBuilder.Entity<Desk>()
                .HasKey(d => d.DeskId);

            modelBuilder.Entity<Desk>()
                .HasMany(d => d.Bookings)
                .WithOne(b => b.Desk)
                .HasForeignKey(d => d.DeskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Desk>()
                .HasOne(d => d.OfficeFloor)
                .WithMany(of => of.Desks)
                .HasForeignKey(d => d.OfficeFloorId)
                .OnDelete(DeleteBehavior.Restrict);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // OfficeFloors

            modelBuilder.Entity<OfficeFloor>()
                .HasKey(of => of.OfficeFloorId);

            modelBuilder.Entity<OfficeFloor>()
                .HasMany(of => of.Desks)
                .WithOne(d => d.OfficeFloor)
                .HasForeignKey(d => d.OfficeFloorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OfficeFloor>()
                .HasOne(of => of.Office)
                .WithMany(o => o.OfficeFloors)
                .HasForeignKey(of => of.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Offices

            modelBuilder.Entity<Office>()
                .HasKey(o => o.OfficeId);

            modelBuilder.Entity<Office>()
                .HasMany(o => o.OfficeFloors)
                .WithOne(of => of.Office)
                .HasForeignKey(of => of.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        }
    }
}
