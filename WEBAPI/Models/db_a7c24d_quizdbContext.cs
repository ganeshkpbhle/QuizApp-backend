using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WEBAPI.Models​
{
    public partial class db_a7c24d_quizdbContext : DbContext
    {
        public db_a7c24d_quizdbContext()
        {
        }

        public db_a7c24d_quizdbContext(DbContextOptions<db_a7c24d_quizdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attempt> Attempts { get; set; }
        public virtual DbSet<QuesDb> QuesDbs { get; set; }
        public virtual DbSet<UserDatum> UserData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SQL5063.site4now.net,1433;Initial Catalog=db_a7c24d_quizdb;User Id=db_a7c24d_quizdb_admin;Password=Ganeshkp@2001");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Attempt>(entity =>
            {
                entity.HasKey(e => e.EntryId);

                entity.ToTable("attempts");

                entity.Property(e => e.EntryId).HasColumnName("entry_id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("_date");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.TimeSpent).HasColumnName("time_spent");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.HasOne(d => d.UIdNavigation)
                    .WithMany(p => p.Attempts)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_attempts_user_data");
            });

            modelBuilder.Entity<QuesDb>(entity =>
            {
                entity.HasKey(e => e.QnId);

                entity.ToTable("QuesDb");

                entity.Property(e => e.Ans).HasColumnName("ans");

                entity.Property(e => e.ImageName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Option1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Option2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Option3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Option4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qn)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserDatum>(entity =>
            {
                entity.HasKey(e => e.UId);

                entity.ToTable("user_data");

                entity.Property(e => e.UId).HasColumnName("u_id");

                entity.Property(e => e.Mobnumber)
                    .HasMaxLength(20)
                    .HasColumnName("mobnumber");

                entity.Property(e => e.Passwd)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("passwd");

                entity.Property(e => e.UEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_email");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
