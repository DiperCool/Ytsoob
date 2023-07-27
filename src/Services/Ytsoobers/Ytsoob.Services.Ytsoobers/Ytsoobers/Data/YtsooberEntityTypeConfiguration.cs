using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Services.Ytsoobers.Shared.Data;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Services.Ytsoobers.Ytsoobers.Data;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Ytsoober>
{
    public void Configure(EntityTypeBuilder<Ytsoober> builder)
    {
        builder.ToTable(nameof(Ytsoober).Pluralize().Underscore(), YtsoobersesDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.OwnsOne(
            x => x.Email,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Ytsoober.Email).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );

        builder.OwnsOne(
            x => x.Username,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Ytsoober.Username).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );
    }
}
