using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Services.Posts.Shared.Data;

namespace Ytsoob.Services.Posts.Poll.Data;

public class PollEntityTypeConfiguration : IEntityTypeConfiguration<Models.Poll>
{
    public void Configure(EntityTypeBuilder<Models.Poll> builder)
    {
        builder.ToTable(nameof(Poll).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.HasOne(x => x.Post).WithOne(x => x.Poll);
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.Navigation(x => x.Options).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
