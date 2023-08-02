using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Services.Posts.Poll.Models;
using Ytsoob.Services.Posts.Shared.Data;

namespace Ytsoob.Services.Posts.Poll.Data;

public class VoterEntityTypeConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        builder.ToTable(nameof(Voter).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
