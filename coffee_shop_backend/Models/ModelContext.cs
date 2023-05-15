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
        public virtual DbSet<Examine> Examines { get; set; } = null!;
        public virtual DbSet<ExamineAnswer> ExamineAnswers { get; set; } = null!;
        public virtual DbSet<ExamineFillInRecord> ExamineFillInRecords { get; set; } = null!;
        public virtual DbSet<ExamineFillInRecordDetail> ExamineFillInRecordDetails { get; set; } = null!;
        public virtual DbSet<ExamineQuestion> ExamineQuestions { get; set; } = null!;
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

            modelBuilder.Entity<Examine>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Caption).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Creator).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ExamineNo)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FooterText).HasMaxLength(2000);

                entity.Property(e => e.HeadText).HasMaxLength(2000);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Updator).HasMaxLength(50);
            });

            modelBuilder.Entity<ExamineAnswer>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Caption).HasMaxLength(30);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExamineQuestionId).HasColumnName("ExamineQuestionID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SeqNo).ValueGeneratedOnAdd();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ExamineFillInRecord>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExamineId).HasColumnName("ExamineID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ip)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.SeqNo).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ExamineFillInRecordDetail>(entity =>
            {
                entity.HasKey(e => e.SeqNo);

                entity.Property(e => e.AnswerText).HasMaxLength(2000);

                entity.Property(e => e.ExamineAnswerId).HasColumnName("ExamineAnswerID");

                entity.Property(e => e.ExamineFillInRecordsId).HasColumnName("ExamineFillInRecordsID");

                entity.Property(e => e.ExamineQuestionId).HasColumnName("ExamineQuestionID");
            });

            modelBuilder.Entity<ExamineQuestion>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Caption).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExamineId).HasColumnName("ExamineID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.SeqNo).ValueGeneratedOnAdd();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
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
