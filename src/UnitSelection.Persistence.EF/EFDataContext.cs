﻿using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Terms;
using UnitSelection.Persistence.EF.Terms;

namespace UnitSelection.Persistence.EF;

public class EFDataContext : DbContext
{
    public EFDataContext(DbContextOptions options) : base(options)
    {
    }

    public EFDataContext(string connectionString) :
        this(new DbContextOptionsBuilder()
            .UseSqlServer(connectionString).Options)
    {
    }

    public DbSet<Term> Terms { get; set; }
    public DbSet<Class> Classes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly
            (typeof(TermEntityMap).Assembly);
    }
}