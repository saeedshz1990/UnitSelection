using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Entities.Terms;
using UnitSelection.Persistence.EF.TermPersistence;

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
    public DbSet<Course> Courses { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ChooseUnit> ChooseUnits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly
            (typeof(TermEntityMap).Assembly);
    }
}