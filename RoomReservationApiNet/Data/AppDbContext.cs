using Microsoft.EntityFrameworkCore;
using RoomReservationApiNet.Models;

namespace RoomReservationApiNet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<EmailQueue> EmailQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Initial data configuration for ReservationStatuses
            modelBuilder.Entity<ReservationStatus>().HasData(
                new ReservationStatus { StatusId = 1, Name = "pending" },
                new ReservationStatus { StatusId = 2, Name = "confirmed" },
                new ReservationStatus { StatusId = 3, Name = "checked-in" },
                new ReservationStatus { StatusId = 4, Name = "checked-out" },
                new ReservationStatus { StatusId = 5, Name = "cancelled" },
                new ReservationStatus { StatusId = 6, Name = "no-show" }
            );

            // Relationships configuration
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.Email)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EmailQueue>()
                .HasOne(e => e.Reservation)
                .WithMany()
                .HasForeignKey(e => e.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);


            // Configuring CHECK restrictions for ReservationStatus
            modelBuilder.Entity<ReservationStatus>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint("CK_ReservationStatus_Name", "Name IN ('pending', 'confirmed', 'checked-in', 'checked-out', 'cancelled', 'no-show')"));
            });


            // Configuring CHECK restrictions for EmailType
            modelBuilder.Entity<EmailQueue>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint("CK_EmailQueue_EmailType", "EmailType IN ('confirmation', 'reminder', 'cancellation', 'notification')"));
            });

            // Configuring CHECK restrictions for Email Status
            modelBuilder.Entity<EmailQueue>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint("CK_EmailQueue_Status", "Status IN ('pending', 'sent', 'failed', 'retrying')"));
            });


            // Configuring CHECK restrictions for User Role
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint("CK_User_Role", "Role IN ('client', 'admin', 'staff')"));
            });
        }
    }
}
