using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Infrastructure.Data.Configurations
{
    public class SecurityConfig : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.ToTable("securities");

            builder.Property(e => e.SecurityId);
            builder.Property(e => e.TypeName);
            builder.Property(e => e.ShortName);
            builder.Property(e => e.Isin);
            builder.Property(e => e.Name);

            builder.HasMany(s => s.Totals).WithOne(t => t.Security);
            builder.HasKey(e => new { e.SecurityId, e.ShortName });
        }
    }
}
