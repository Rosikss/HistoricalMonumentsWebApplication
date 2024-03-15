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

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
