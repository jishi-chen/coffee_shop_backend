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
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<DocumentField> DocumentFields { get; set; } = null!;
        public virtual DbSet<DocumentFieldOption> DocumentFieldOptions { get; set; } = null!;
        public virtual DbSet<DocumentRecord> DocumentRecords { get; set; } = null!;
        public virtual DbSet<MemberInfo> MemberInfos { get; set; } = null!;
        public virtual DbSet<Reg> Regs { get; set; } = null!;

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

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Caption).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(20);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Updator).HasMaxLength(20);
            });

            modelBuilder.Entity<DocumentField>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.FieldName).HasMaxLength(50);

                entity.Property(e => e.FileExtension)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Updator).HasMaxLength(50);
            });

            modelBuilder.Entity<DocumentFieldOption>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.OptionName).HasMaxLength(50);
            });

            modelBuilder.Entity<DocumentRecord>(entity =>
            {
                entity.HasKey(e => e.SeqNo);

                entity.Property(e => e.SeqNo).ValueGeneratedNever();

                entity.Property(e => e.FilledText).HasColumnType("ntext");

                entity.Property(e => e.MemoText).HasMaxLength(100);

                entity.Property(e => e.Remark).HasMaxLength(1000);
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

            modelBuilder.Entity<Reg>(entity =>
            {
                entity.ToTable("Reg");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
