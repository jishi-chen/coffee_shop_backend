using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace coffee_shop_backend.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressArea> AddressAreas { get; set; } = null!;
        public virtual DbSet<AddressCity> AddressCities { get; set; } = null!;
        public virtual DbSet<MemberInfo> MemberInfos { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressArea>(entity =>
            {
                entity.ToTable("AddressArea");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AreaName).HasMaxLength(5);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.SortIndex).ValueGeneratedOnAdd();

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AddressCity>(entity =>
            {
                entity.ToTable("AddressCity");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CityName).HasMaxLength(5);

                entity.Property(e => e.SortIndex).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MemberInfo>(entity =>
            {
                entity.ToTable("MemberInfo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
