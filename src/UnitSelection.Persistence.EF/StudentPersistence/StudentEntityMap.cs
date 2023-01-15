using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitSelection.Entities.Students;

namespace UnitSelection.Persistence.EF.StudentPersistence;

public class StudentEntityMap: IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id).ValueGeneratedOnAdd();
        
        builder.Property(_ => _.FirstName);
        builder.Property(_ => _.LastName);
        builder.Property(_ => _.FatherName);
        builder.Property(_ => _.NationalCode);
        builder.Property(_ => _.DateOfBirth);
        builder.Property(_ => _.Address);
        
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