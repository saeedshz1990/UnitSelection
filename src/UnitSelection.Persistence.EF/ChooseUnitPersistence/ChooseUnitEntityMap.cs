using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitSelection.Entities.ChooseUnits;

namespace UnitSelection.Persistence.EF.ChooseUnitPersistence;

public class ChooseUnitEntityMap : IEntityTypeConfiguration<ChooseUnit>
{
    public void Configure(EntityTypeBuilder<ChooseUnit> builder)
    {
        builder.ToTable("ChooseUnits");

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Id).ValueGeneratedOnAdd();
    }
}