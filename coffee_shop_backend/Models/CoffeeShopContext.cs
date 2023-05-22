using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace coffee_shop_backend.Models
{
    public partial class CoffeeShopContext : DbContext
    {
        public CoffeeShopContext()
        {
        }

        public CoffeeShopContext(DbContextOptions<CoffeeShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationRecord> ApplicationRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRecord>(entity =>
            {
                entity.HasKey(e => e.SeqNo)
                    .HasName("PK_ApplicationRecordDetails");

                entity.Property(e => e.AnswerText).HasMaxLength(2000);

                entity.Property(e => e.ApplicationFieldId).HasColumnName("ApplicationFieldID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.FilledText).HasMaxLength(1000);

                entity.Property(e => e.Remark).HasMaxLength(1000);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
