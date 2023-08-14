﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ytsoob.Services.Posts.Shared.Data;

#nullable disable

namespace Ytsoob.Services.Posts.Migrations
{
    [DbContext(typeof(PostsDbContext))]
    [Migration("20230814073314_Subscriptions")]
    partial class Subscriptions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ytsoob.Services.Posts.Comments.Models.BaseComment", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<List<string>>("Files")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("files");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_id");

                    b.Property<long>("ReactionStatsId")
                        .HasColumnType("bigint")
                        .HasColumnName("reaction_stats_id");

                    b.HasKey("Id")
                        .HasName("pk_comments");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_comments_id");

                    b.HasIndex("ReactionStatsId")
                        .HasDatabaseName("ix_comments_reaction_stats_id");

                    b.ToTable("comments", "posts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseComment");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Contents.Models.Content", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<List<string>>("Files")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("files");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_id");

                    b.HasKey("Id")
                        .HasName("pk_contents");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_contents_id");

                    b.HasIndex("PostId")
                        .IsUnique()
                        .HasDatabaseName("ix_contents_post_id");

                    b.ToTable("contents", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Option", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<long>("PollId")
                        .HasColumnType("bigint")
                        .HasColumnName("poll_id");

                    b.HasKey("Id")
                        .HasName("pk_options");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_options_id");

                    b.HasIndex("PollId")
                        .HasDatabaseName("ix_options_poll_id");

                    b.ToTable("options", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Poll", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<string>("PollAnswerType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("poll_answer_type");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint")
                        .HasColumnName("post_id");

                    b.HasKey("Id")
                        .HasName("pk_polls");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_polls_id");

                    b.HasIndex("PostId")
                        .IsUnique()
                        .HasDatabaseName("ix_polls_post_id");

                    b.ToTable("polls", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Voter", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<long>("OptionId")
                        .HasColumnType("bigint")
                        .HasColumnName("option_id");

                    b.Property<long>("YtsooberId")
                        .HasColumnType("bigint")
                        .HasColumnName("ytsoober_id");

                    b.HasKey("Id")
                        .HasName("pk_voters");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_voters_id");

                    b.HasIndex("OptionId")
                        .HasDatabaseName("ix_voters_option_id");

                    b.HasIndex("YtsooberId")
                        .HasDatabaseName("ix_voters_ytsoober_id");

                    b.ToTable("voters", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Posts.Models.Post", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.Property<long>("ReactionStatsId")
                        .HasColumnType("bigint")
                        .HasColumnName("reaction_stats_id");

                    b.Property<long?>("SubscriptionId")
                        .HasColumnType("bigint")
                        .HasColumnName("subscription_id");

                    b.HasKey("Id")
                        .HasName("pk_posts");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_posts_id");

                    b.HasIndex("ReactionStatsId")
                        .HasDatabaseName("ix_posts_reaction_stats_id");

                    b.HasIndex("SubscriptionId")
                        .HasDatabaseName("ix_posts_subscription_id");

                    b.ToTable("posts", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Reactions.Models.Reaction", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("entity_id");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("entity_type");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.Property<int>("ReactionType")
                        .HasColumnType("integer")
                        .HasColumnName("reaction_type");

                    b.Property<long>("YtsooberId")
                        .HasColumnType("bigint")
                        .HasColumnName("ytsoober_id");

                    b.HasKey("Id")
                        .HasName("pk_reactions");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_reactions_id");

                    b.HasIndex("YtsooberId")
                        .HasDatabaseName("ix_reactions_ytsoober_id");

                    b.ToTable("reactions", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Reactions.Models.ReactionStats", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.HasKey("Id")
                        .HasName("pk_reaction_stats");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_reaction_stats_id");

                    b.ToTable("reaction_stats", "posts");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Subscriptions.Models.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Photo")
                        .HasColumnType("text")
                        .HasColumnName("photo");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<long>("YtsooberId")
                        .HasColumnType("bigint")
                        .HasColumnName("ytsoober_id");

                    b.HasKey("Id")
                        .HasName("pk_subscriptions");

                    b.HasIndex("YtsooberId")
                        .HasDatabaseName("ix_subscriptions_ytsoober_id");

                    b.ToTable("subscriptions", (string)null);
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Users.Features.Models.Ytsoober", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("text")
                        .HasColumnName("avatar");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uuid")
                        .HasColumnName("identity_id");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_ytsoobers");

                    b.ToTable("ytsoobers", (string)null);
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Comments.Models.Comment", b =>
                {
                    b.HasBaseType("Ytsoob.Services.Posts.Comments.Models.BaseComment");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Comments.Models.RepliedComment", b =>
                {
                    b.HasBaseType("Ytsoob.Services.Posts.Comments.Models.BaseComment");

                    b.Property<long>("CommentId")
                        .HasColumnType("bigint")
                        .HasColumnName("comment_id");

                    b.Property<long>("RepliedToCommentId")
                        .HasColumnType("bigint")
                        .HasColumnName("replied_to_comment_id");

                    b.HasIndex("CommentId")
                        .HasDatabaseName("ix_base_comments_comment_id");

                    b.HasIndex("RepliedToCommentId")
                        .HasDatabaseName("ix_base_comments_replied_to_comment_id");

                    b.HasDiscriminator().HasValue("RepliedComment");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Comments.Models.BaseComment", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Reactions.Models.ReactionStats", "ReactionStats")
                        .WithMany()
                        .HasForeignKey("ReactionStatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_comments_reaction_stats_reaction_stats_id");

                    b.OwnsOne("Ytsoob.Services.Posts.Comments.ValueObjects.CommentContent", "Content", b1 =>
                        {
                            b1.Property<long>("BaseCommentId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("comment_content");

                            b1.HasKey("BaseCommentId");

                            b1.ToTable("comments", "posts");

                            b1.WithOwner()
                                .HasForeignKey("BaseCommentId")
                                .HasConstraintName("fk_comments_comments_id");
                        });

                    b.Navigation("Content")
                        .IsRequired();

                    b.Navigation("ReactionStats");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Contents.Models.Content", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Posts.Models.Post", null)
                        .WithOne("Content")
                        .HasForeignKey("Ytsoob.Services.Posts.Contents.Models.Content", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_contents_posts_post_id");

                    b.OwnsOne("Ytsoob.Services.Posts.Contents.ValueObjects.ContentText", "ContentText", b1 =>
                        {
                            b1.Property<long>("ContentId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("content_text");

                            b1.HasKey("ContentId");

                            b1.ToTable("contents", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ContentId")
                                .HasConstraintName("fk_contents_contents_id");
                        });

                    b.Navigation("ContentText")
                        .IsRequired();
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Option", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Polls.Models.Poll", "Poll")
                        .WithMany("Options")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_options_polls_poll_id");

                    b.OwnsOne("Ytsoob.Services.Posts.Polls.ValueObjects.Fiction", "Fiction", b1 =>
                        {
                            b1.Property<long>("OptionId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric")
                                .HasColumnName("fiction");

                            b1.HasKey("OptionId");

                            b1.ToTable("options", "posts");

                            b1.WithOwner()
                                .HasForeignKey("OptionId")
                                .HasConstraintName("fk_options_options_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Polls.ValueObjects.OptionCount", "Count", b1 =>
                        {
                            b1.Property<long>("OptionId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .HasColumnType("bigint")
                                .HasColumnName("option_count");

                            b1.HasKey("OptionId");

                            b1.ToTable("options", "posts");

                            b1.WithOwner()
                                .HasForeignKey("OptionId")
                                .HasConstraintName("fk_options_options_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Polls.ValueObjects.OptionTitle", "Title", b1 =>
                        {
                            b1.Property<long>("OptionId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("option_title");

                            b1.HasKey("OptionId");

                            b1.ToTable("options", "posts");

                            b1.WithOwner()
                                .HasForeignKey("OptionId")
                                .HasConstraintName("fk_options_options_id");
                        });

                    b.Navigation("Count")
                        .IsRequired();

                    b.Navigation("Fiction")
                        .IsRequired();

                    b.Navigation("Poll");

                    b.Navigation("Title")
                        .IsRequired();
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Poll", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Posts.Models.Post", "Post")
                        .WithOne("Poll")
                        .HasForeignKey("Ytsoob.Services.Posts.Polls.Models.Poll", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_polls_posts_post_id");

                    b.OwnsOne("Ytsoob.Services.Posts.Polls.ValueObjects.Question", "Question", b1 =>
                        {
                            b1.Property<long>("PollId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("question");

                            b1.HasKey("PollId");

                            b1.ToTable("polls", "posts");

                            b1.WithOwner()
                                .HasForeignKey("PollId")
                                .HasConstraintName("fk_polls_polls_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Polls.ValueObjects.TotalCountPoll", "TotalCountPoll", b1 =>
                        {
                            b1.Property<long>("PollId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .HasColumnType("bigint")
                                .HasColumnName("total_count_poll");

                            b1.HasKey("PollId");

                            b1.ToTable("polls", "posts");

                            b1.WithOwner()
                                .HasForeignKey("PollId")
                                .HasConstraintName("fk_polls_polls_id");
                        });

                    b.Navigation("Post");

                    b.Navigation("Question")
                        .IsRequired();

                    b.Navigation("TotalCountPoll")
                        .IsRequired();
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Voter", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Polls.Models.Option", "Option")
                        .WithMany()
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_voters_options_option_id");

                    b.HasOne("Ytsoob.Services.Posts.Users.Features.Models.Ytsoober", "Ytsoober")
                        .WithMany()
                        .HasForeignKey("YtsooberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_voters_ytsoobers_ytsoober_id");

                    b.Navigation("Option");

                    b.Navigation("Ytsoober");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Posts.Models.Post", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Reactions.Models.ReactionStats", "ReactionStats")
                        .WithMany()
                        .HasForeignKey("ReactionStatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_posts_reaction_stats_reaction_stats_id");

                    b.HasOne("Ytsoob.Services.Posts.Subscriptions.Models.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId")
                        .HasConstraintName("fk_posts_subscriptions_subscription_id");

                    b.Navigation("ReactionStats");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Reactions.Models.Reaction", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Users.Features.Models.Ytsoober", "Ytsoober")
                        .WithMany()
                        .HasForeignKey("YtsooberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_reactions_ytsoobers_ytsoober_id");

                    b.Navigation("Ytsoober");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Reactions.Models.ReactionStats", b =>
                {
                    b.OwnsOne("Ytsoob.Services.Posts.Reactions.ValueObjects.ReactionNumber", "Angry", b1 =>
                        {
                            b1.Property<long>("ReactionStatsId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .HasColumnType("bigint")
                                .HasColumnName("angry");

                            b1.HasKey("ReactionStatsId");

                            b1.ToTable("reaction_stats", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ReactionStatsId")
                                .HasConstraintName("fk_reaction_stats_reaction_stats_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Reactions.ValueObjects.ReactionNumber", "Crying", b1 =>
                        {
                            b1.Property<long>("ReactionStatsId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .HasColumnType("bigint")
                                .HasColumnName("crying");

                            b1.HasKey("ReactionStatsId");

                            b1.ToTable("reaction_stats", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ReactionStatsId")
                                .HasConstraintName("fk_reaction_stats_reaction_stats_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Reactions.ValueObjects.ReactionNumber", "Dislike", b1 =>
                        {
                            b1.Property<long>("ReactionStatsId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .ValueGeneratedOnUpdateSometimes()
                                .HasColumnType("bigint")
                                .HasColumnName("dislike");

                            b1.HasKey("ReactionStatsId");

                            b1.ToTable("reaction_stats", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ReactionStatsId")
                                .HasConstraintName("fk_reaction_stats_reaction_stats_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Reactions.ValueObjects.ReactionNumber", "Happy", b1 =>
                        {
                            b1.Property<long>("ReactionStatsId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .ValueGeneratedOnUpdateSometimes()
                                .HasColumnType("bigint")
                                .HasColumnName("dislike");

                            b1.HasKey("ReactionStatsId");

                            b1.ToTable("reaction_stats", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ReactionStatsId")
                                .HasConstraintName("fk_reaction_stats_reaction_stats_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Reactions.ValueObjects.ReactionNumber", "Like", b1 =>
                        {
                            b1.Property<long>("ReactionStatsId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .HasColumnType("bigint")
                                .HasColumnName("like");

                            b1.HasKey("ReactionStatsId");

                            b1.ToTable("reaction_stats", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ReactionStatsId")
                                .HasConstraintName("fk_reaction_stats_reaction_stats_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Posts.Reactions.ValueObjects.ReactionNumber", "Wonder", b1 =>
                        {
                            b1.Property<long>("ReactionStatsId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("Value")
                                .HasColumnType("bigint")
                                .HasColumnName("wonder");

                            b1.HasKey("ReactionStatsId");

                            b1.ToTable("reaction_stats", "posts");

                            b1.WithOwner()
                                .HasForeignKey("ReactionStatsId")
                                .HasConstraintName("fk_reaction_stats_reaction_stats_id");
                        });

                    b.Navigation("Angry")
                        .IsRequired();

                    b.Navigation("Crying")
                        .IsRequired();

                    b.Navigation("Dislike")
                        .IsRequired();

                    b.Navigation("Happy")
                        .IsRequired();

                    b.Navigation("Like")
                        .IsRequired();

                    b.Navigation("Wonder")
                        .IsRequired();
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Subscriptions.Models.Subscription", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Users.Features.Models.Ytsoober", "Ytsoober")
                        .WithMany()
                        .HasForeignKey("YtsooberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_subscriptions_ytsoobers_ytsoober_id");

                    b.Navigation("Ytsoober");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Comments.Models.RepliedComment", b =>
                {
                    b.HasOne("Ytsoob.Services.Posts.Comments.Models.Comment", "Comment")
                        .WithMany("RepliedComments")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_base_comments_base_comments_comment_id");

                    b.HasOne("Ytsoob.Services.Posts.Comments.Models.BaseComment", "RepliedToComment")
                        .WithMany()
                        .HasForeignKey("RepliedToCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_base_comments_base_comments_replied_to_comment_id");

                    b.Navigation("Comment");

                    b.Navigation("RepliedToComment");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Polls.Models.Poll", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Posts.Models.Post", b =>
                {
                    b.Navigation("Content")
                        .IsRequired();

                    b.Navigation("Poll");
                });

            modelBuilder.Entity("Ytsoob.Services.Posts.Comments.Models.Comment", b =>
                {
                    b.Navigation("RepliedComments");
                });
#pragma warning restore 612, 618
        }
    }
}