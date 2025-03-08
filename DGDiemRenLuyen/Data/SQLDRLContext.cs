using System;
using System.Collections.Generic;
using DGDiemRenLuyen.Models;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Data;

public partial class SQLDRLContext : DbContext
{
    private readonly IConfiguration _configuration;

    public SQLDRLContext()
    {
    }

    public SQLDRLContext(DbContextOptions<SQLDRLContext> options, IConfiguration configuration)
        : base(options)  
    {
        _configuration = configuration;
    }

    public virtual DbSet<ChildCriterion> ChildCriteria { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<CriteriaDetail> CriteriaDetails { get; set; }

    public virtual DbSet<ParentCriterion> ParentCriteria { get; set; }

    public virtual DbSet<ScoreStatus> Scores { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Time> Times { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Cnn"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChildCriterion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__childCri__3213E83FD95846FD");

            entity.HasOne(d => d.ParentCriteria).WithMany(p => p.ChildCriteria).HasConstraintName("FK__childCrit__paren__45F365D3");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__class__7577347E59807545");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes).HasConstraintName("FK__class__teacherId__47DBAE45");
        });

        modelBuilder.Entity<CriteriaDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__criteria__3213E83F6272ADF0");

            entity.HasOne(d => d.ChildCriteria).WithMany(p => p.CriteriaDetails).HasConstraintName("FK__criteriaD__child__46E78A0C");

            entity.HasOne(d => d.Score).WithMany(p => p.CriteriaDetails).HasConstraintName("FK__criteriaD__score__48CFD27E");
        });

        modelBuilder.Entity<ParentCriterion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__parentCr__3213E83F1F9F1016");
        });

        modelBuilder.Entity<ScoreStatus>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PK__score__B56A0C8DC3C4958E");

            entity.HasOne(d => d.Student).WithMany(p => p.Scores).HasConstraintName("FK__score__studentId__4AB81AF0");

            entity.HasOne(d => d.Time).WithMany(p => p.Scores).HasConstraintName("FK__score__timeId__44FF419A");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__students__4D11D63CC7ABE612");

            entity.HasOne(d => d.Class).WithMany(p => p.Students).HasConstraintName("FK__students__classI__49C3F6B7");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__teachers__98E93895018A7EA3");
        });

        modelBuilder.Entity<Time>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__times__3213E83F54CCCAED");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow.AddHours(7); // Khi tạo mới
            }
            entity.UpdatedAt = DateTime.UtcNow.AddHours(7); ; // Khi cập nhật
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow.AddHours(7); ; // Khi tạo mới
            }
            entity.UpdatedAt = DateTime.UtcNow.AddHours(7); ; // Khi cập nhật
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
