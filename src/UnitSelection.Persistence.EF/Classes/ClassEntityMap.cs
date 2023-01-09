using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitSelection.Entities.Classes;

namespace UnitSelection.Persistence.EF.Classes
{
    public class ClassEntityMap : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("Classes");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name);

            builder.HasOne(_ => _.Term)
                .WithMany(_ => _.Class)
                .HasForeignKey(x => x.TermId);
        }
    }
}
