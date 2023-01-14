using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitSelection.Entities.Teachers;

namespace UnitSelection.Persistence.EF.TeacherPersistence;

public class TeacherEntityMap : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id).ValueGeneratedOnAdd();

        builder.Property(_ => _.FirstName);
        builder.Property(_ => _.LastName);
        builder.Property(_ => _.FatherName);
        builder.Property(_ => _.NationalCode);
        builder.Property(_ => _.DateOfBirth);
        builder.Property(_ => _.Address);
        builder.Property(_ => _.GroupOfCourse);
        builder.Property(_ => _.Study);
        builder.Property(_ => _.Diploma);

        builder.OwnsOne(_ => _.Mobile, _ =>
        {
            _.Property(_ => _.CountryCallingCode)
                .HasColumnName("CountryCallingCode")
                .IsRequired();
            _.Property(_ => _.MobileNumber)
                .HasColumnName("MobileNumber")
                .IsRequired();
        });
    }
}