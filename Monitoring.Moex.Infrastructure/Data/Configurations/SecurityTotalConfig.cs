using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Infrastructure.Data.Configurations
{
    internal class SecurityTotalConfig : IEntityTypeConfiguration<SecurityTotal>
    {
        public void Configure(EntityTypeBuilder<SecurityTotal> builder)
        {
            builder.ToTable("light_security_totals");

            builder.Property(t => t.TradeClock);
            builder.Property(t => t.Open);
            builder.Property(t => t.Close);
            builder.Property(t => t.High);
            builder.Property(t => t.Low);
            builder.Property(t => t.OpenCloseDelta);
            builder.Property(t => t.SecurityId);

            builder.HasOne(t => t.Security).WithMany(s => s.Totals).HasPrincipalKey(t => t.SecurityId);
            builder.HasKey(t => new { t.SecurityId, t.TradeClock });
        }
    }
}
