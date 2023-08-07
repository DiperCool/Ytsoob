using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Shared.Data;

namespace Ytsoob.Services.Posts.Comments.Data;

public class BaseCommentEntityConfiguration : IEntityTypeConfiguration<BaseComment>
{
    public const string TableName = "comments";

    public virtual void Configure(EntityTypeBuilder<BaseComment> builder)
    {
        builder.ToTable(TableName, PostsDbContext.DefaultSchema);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.OwnsOne(
            x => x.Content,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.CommentContent).Underscore()).IsRequired();
            }
        );
    }
}
