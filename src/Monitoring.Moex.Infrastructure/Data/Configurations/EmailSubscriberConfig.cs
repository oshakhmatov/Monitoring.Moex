using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Infrastructure.Data.Configurations
{
    internal class EmailSubscriberConfig : IEntityTypeConfiguration<EmailSubscriber>
    {
        public void Configure(EntityTypeBuilder<EmailSubscriber> builder)
        {
            builder.ToTable("email_subscribers");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Email);
            builder.Property(e => e.Received);

            builder.HasKey(e => e.Id);
        }
    }
}
