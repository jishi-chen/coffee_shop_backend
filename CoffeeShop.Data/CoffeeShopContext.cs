using System;
using System.Collections.Generic;
using CoffeeShop.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Model;

public partial class CoffeeShopContext : DbContext
{
    public CoffeeShopContext(DbContextOptions<CoffeeShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddressArea> AddressAreas { get; set; }

    public virtual DbSet<AddressCity> AddressCities { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentField> DocumentFields { get; set; }

    public virtual DbSet<DocumentFieldOption> DocumentFieldOptions { get; set; }

    public virtual DbSet<DocumentRecord> DocumentRecords { get; set; }

    public virtual DbSet<MemberInfo> MemberInfos { get; set; }

    public virtual DbSet<Reg> Regs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressArea>(entity =>
        {
            entity.ToTable("AddressArea");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AreaName).HasMaxLength(50);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AddressCity>(entity =>
        {
            entity.ToTable("AddressCity");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.Caption).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Updator)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentField>(entity =>
        {
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FieldName).HasMaxLength(50);
            entity.Property(e => e.FileExtension)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Updator)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentFieldOption>(entity =>
        {
            entity.Property(e => e.OptionName).HasMaxLength(50);
        });

        modelBuilder.Entity<DocumentRecord>(entity =>
        {
            entity.HasKey(e => e.SeqNo).HasName("PK_DocumentRecord");

            entity.Property(e => e.FilledText).HasMaxLength(50);
            entity.Property(e => e.MemoText).HasMaxLength(50);
            entity.Property(e => e.Remark).HasMaxLength(50);
        });

        modelBuilder.Entity<MemberInfo>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Reg>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
