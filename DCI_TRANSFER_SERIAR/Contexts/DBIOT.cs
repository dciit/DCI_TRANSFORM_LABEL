using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DCI_TRANSFER_SERIAR.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DCI_TRANSFER_SERIAR.Contexts
{
    public partial class DBIOT : DbContext
    {
        public DBIOT()
        {
        }

        public DBIOT(DbContextOptions<DBIOT> options)
            : base(options)
        {
        }

        public virtual DbSet<MasterDigit> MasterDigit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=192.168.226.145;Database=dbIot;TrustServerCertificate=True;uid=sa;password=decjapan");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasterDigit>(entity =>
            {
                entity.HasKey(e => e.Digit)
                    .HasName("PK_Master_Digit3");

                entity.ToTable("Master_Digit");

                entity.Property(e => e.Digit).HasMaxLength(50);

                entity.Property(e => e.DigitValue)
                    .HasColumnName("Digit_Value")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
