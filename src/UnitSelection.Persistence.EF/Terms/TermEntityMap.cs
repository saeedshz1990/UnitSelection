using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitSelection.Entities.Terms;

namespace UnitSelection.Persistence.EF.Terms
{
    public class TermEntityMap : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder.ToTable("Terms");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.Name).IsRequired();
            builder.Property(_ => _.StartDate);
            builder.Property(_ => _.EndDate);
        }
    }
}
