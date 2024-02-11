using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HistoricalMonumentsWebApplication.Models;

public partial class DblibraryContext : DbContext
{
    public DblibraryContext()
    {
    }

    public DblibraryContext(DbContextOptions<DblibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Architect> Architects { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Classification> Classifications { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<HistoricalMonument> HistoricalMonuments { get; set; }

    public virtual DbSet<HistoricalMonumentArchitect> HistoricalMonumentArchitects { get; set; }

    public virtual DbSet<HistoricalMonumentMaterial> HistoricalMonumentMaterials { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-D8QO0U2\\SQLEXPRESS; Database=DBLibrary; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Architect>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Architects)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Architects_Countries");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cities_Countries");
        });

        modelBuilder.Entity<Classification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Styles");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<HistoricalMonument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Historical_Monuments");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.City).WithMany(p => p.HistoricalMonuments)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historical_Monuments_Cities");

            entity.HasOne(d => d.Classification).WithMany(p => p.HistoricalMonuments)
                .HasForeignKey(d => d.ClassificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historical_Monuments_Styles");

            entity.HasOne(d => d.Status).WithMany(p => p.HistoricalMonuments)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricalMonuments_Statuses");
        });

        modelBuilder.Entity<HistoricalMonumentArchitect>(entity =>
        {
            entity.HasOne(d => d.Architect).WithMany(p => p.HistoricalMonumentArchitects)
                .HasForeignKey(d => d.ArchitectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricalMonumentArchitects_Architects");

            entity.HasOne(d => d.HistoricalMonument).WithMany(p => p.HistoricalMonumentArchitects)
                .HasForeignKey(d => d.HistoricalMonumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricalMonumentArchitects_HistoricalMonuments");
        });

        modelBuilder.Entity<HistoricalMonumentMaterial>(entity =>
        {
            entity.HasOne(d => d.HistoricalMonument).WithMany(p => p.HistoricalMonumentMaterials)
                .HasForeignKey(d => d.HistoricalMonumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricalMonumentMaterials_HistoricalMonumentMaterials");

            entity.HasOne(d => d.Material).WithMany(p => p.HistoricalMonumentMaterials)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoricalMonumentMaterials_Materials1");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
