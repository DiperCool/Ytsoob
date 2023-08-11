using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Data;

namespace Ytsoob.Services.Posts.Posts.Data;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(nameof(Post).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

                // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);

    }
}
