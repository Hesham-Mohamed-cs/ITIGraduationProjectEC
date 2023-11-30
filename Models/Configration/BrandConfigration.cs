using GraduaitionProjectITI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GraduaitionProjectITI.Models.Configration
{
    public class BrandConfigration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasIndex(b => b.Name).IsUnique();
        }
    }
}
