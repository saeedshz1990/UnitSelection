using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SQLitePCL;
using UnitSelection.Entities.Courses;

namespace UnitSelection.Persistence.EF.CoursesPersistence;

public class CourseEntityMap:IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey("Id");
        builder.Property(_ => _.Id).ValueGeneratedOnAdd();

        builder.Property(_ => _.Name).IsRequired();
        builder.Property(_ => _.StartHour);
        builder.Property(_ => _.EndHour);
        builder.Property(_ => _.UnitCount);
        builder.Property(_ => _.DayOfWeek);

        builder.HasOne(_ => _.Class)
            .WithMany(_ => _.Courses)
            .HasForeignKey(_ => _.ClassId);
    }
}