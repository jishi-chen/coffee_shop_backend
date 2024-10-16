﻿using System;
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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentField> DocumentFields { get; set; }

    public virtual DbSet<DocumentFieldOption> DocumentFieldOptions { get; set; }

    public virtual DbSet<DocumentRecord> DocumentRecords { get; set; }

    public virtual DbSet<DocumentRecordDetail> DocumentRecordDetails { get; set; }

    public virtual DbSet<FileStorage> FileStorages { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressArea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AddressArea_1");

            entity.ToTable("AddressArea");

            entity.Property(e => e.AreaName).HasMaxLength(50);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AddressCity>(entity =>
        {
            entity.ToTable("AddressCity");

            entity.Property(e => e.CityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B5A7E0A9C");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
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
            entity.HasKey(e => e.Id).HasName("PK_Regs");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<DocumentRecordDetail>(entity =>
        {
            entity.HasKey(e => e.SeqNo).HasName("PK_DocumentRecord");

            entity.Property(e => e.FilledText).HasMaxLength(50);
            entity.Property(e => e.MemoText).HasMaxLength(50);
            entity.Property(e => e.Remark).HasMaxLength(50);
        });

        modelBuilder.Entity<FileStorage>(entity =>
        {
            entity.Property(e => e.CategoryType).HasMaxLength(50);
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.ModuleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NewFileName).HasMaxLength(255);
            entity.Property(e => e.OriginalFileName).HasMaxLength(255);
            entity.Property(e => e.UploadDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MemberInfos");

            entity.Property(e => e.Id).ValueGeneratedNever();
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

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Modules__2B7477A7B3A3BDD5");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModuleName).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.ContactName).HasMaxLength(100);
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.TenantName).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
