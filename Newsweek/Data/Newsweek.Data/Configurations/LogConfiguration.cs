namespace Newsweek.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Newsweek.Data.Models;

    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> log)
        {
            log.Property(x => x.Operation)
                .IsRequired();

            log.Property(x => x.Request)
                .IsRequired();
        }
    }
}