﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ytsoob.Services.Subscriptions.Shared.Data;

#nullable disable

namespace Ytsoob.Services.Payment.Migrations
{
    [DbContext(typeof(SubscriptionsDbContext))]
    [Migration("20230810190341_Subscriptions")]
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

            modelBuilder.Entity("Ytsoob.Services.Subscriptions.Subscriptions.Models.Subscription", b =>
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

                    b.Property<string>("Photo")
                        .HasColumnType("text")
                        .HasColumnName("photo");

                    b.Property<long>("YtsooberId")
                        .HasColumnType("bigint")
                        .HasColumnName("ytsoober_id");

                    b.HasKey("Id")
                        .HasName("pk_subscriptions");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_subscriptions_id");

                    b.HasIndex("YtsooberId")
                        .HasDatabaseName("ix_subscriptions_ytsoober_id");

                    b.ToTable("subscriptions", "subscriptions");
                });

            modelBuilder.Entity("Ytsoob.Services.Subscriptions.Ytsoobers.Models.Ytsoober", b =>
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

            modelBuilder.Entity("Ytsoob.Services.Subscriptions.Subscriptions.Models.Subscription", b =>
                {
                    b.HasOne("Ytsoob.Services.Subscriptions.Ytsoobers.Models.Ytsoober", "Ytsoober")
                        .WithMany()
                        .HasForeignKey("YtsooberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_subscriptions_ytsoobers_ytsoober_id");

                    b.OwnsOne("Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects.Description", "Description", b1 =>
                        {
                            b1.Property<long>("SubscriptionId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("description");

                            b1.HasKey("SubscriptionId");

                            b1.ToTable("subscriptions", "subscriptions");

                            b1.WithOwner()
                                .HasForeignKey("SubscriptionId")
                                .HasConstraintName("fk_subscriptions_subscriptions_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects.Price", "Price", b1 =>
                        {
                            b1.Property<long>("SubscriptionId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric")
                                .HasColumnName("price");

                            b1.HasKey("SubscriptionId");

                            b1.ToTable("subscriptions", "subscriptions");

                            b1.WithOwner()
                                .HasForeignKey("SubscriptionId")
                                .HasConstraintName("fk_subscriptions_subscriptions_id");
                        });

                    b.OwnsOne("Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects.Title", "Title", b1 =>
                        {
                            b1.Property<long>("SubscriptionId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("title");

                            b1.HasKey("SubscriptionId");

                            b1.ToTable("subscriptions", "subscriptions");

                            b1.WithOwner()
                                .HasForeignKey("SubscriptionId")
                                .HasConstraintName("fk_subscriptions_subscriptions_id");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Price")
                        .IsRequired();

                    b.Navigation("Title")
                        .IsRequired();

                    b.Navigation("Ytsoober");
                });
#pragma warning restore 612, 618
        }
    }
}
