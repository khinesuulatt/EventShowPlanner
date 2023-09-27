using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventShow.Models
{
    public partial class EventShowPlannerContext : DbContext
    {
        public EventShowPlannerContext()
        {
        }

        public EventShowPlannerContext(DbContextOptions<EventShowPlannerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<EventCategory> EventCategories { get; set; } = null!;
        public virtual DbSet<EventDetail> EventDetails { get; set; } = null!;
        public virtual DbSet<Speaker> Speakers { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=EventShowPlanner;Persist Security Info=False;User ID=SA;Password=RPSsql12345;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.CcreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CCreatedDate");

                entity.Property(e => e.CityName).HasMaxLength(100);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EventCreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EventDate).HasColumnType("date");

                entity.Property(e => e.EventEndTime).HasColumnType("datetime");

                entity.Property(e => e.EventImgPath).HasMaxLength(200);

                entity.Property(e => e.EventLocation).HasMaxLength(100);

                entity.Property(e => e.EventName).HasMaxLength(100);

                entity.Property(e => e.EventStartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.ToTable("EventCategory");

                entity.Property(e => e.EcategoryName)
                    .HasMaxLength(100)
                    .HasColumnName("ECategoryName");

                entity.Property(e => e.EccreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ECCreatedDate");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.ToTable("EventDetail");

                entity.Property(e => e.Edcontent)
                    .HasMaxLength(100)
                    .HasColumnName("EDContent");

                entity.Property(e => e.Edtitle)
                    .HasMaxLength(100)
                    .HasColumnName("EDTitle");
            });

            modelBuilder.Entity<Speaker>(entity =>
            {
                entity.ToTable("Speaker");

                entity.Property(e => e.Description);

                entity.Property(e => e.Professional).HasMaxLength(500);

                entity.Property(e => e.Saddress)
                    .HasMaxLength(200)
                    .HasColumnName("SAddress");

                entity.Property(e => e.Sage).HasColumnName("SAge");

                entity.Property(e => e.ScreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("SCreatedDate");

                entity.Property(e => e.Sjob)
                    .HasMaxLength(200)
                    .HasColumnName("SJob");

                entity.Property(e => e.SpeakerName).HasMaxLength(100);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Qrcode)
                    .HasMaxLength(500)
                    .HasColumnName("QRCode");

                entity.Property(e => e.Qrimgpath)
                    .HasMaxLength(500)
                    .HasColumnName("QRimgpath");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.JobTitle).HasMaxLength(50);

                entity.Property(e => e.UcreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UCreatedDate");

                entity.Property(e => e.UserAddress).HasMaxLength(200);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.AdminName).HasMaxLength(100);
                entity.Property(e => e.AdminPassword).HasMaxLength(100);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FName).HasMaxLength(500);
                entity.Property(e => e.FDate)
                    .HasColumnType("datetime")
                    .HasColumnName("FDate");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
