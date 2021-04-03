﻿// <auto-generated />
using System;
using GoodNewsAggregator.DAL.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GoodNewsAggregator.DAL.Core.Migrations
{
    [DbContext(typeof(GoodNewsAggregatorContext))]
    partial class GoodNewsAggregatorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("GoodFactor")
                        .HasColumnType("real");

                    b.Property<Guid>("RssId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RssId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Rss", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rss");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("RoleId")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("RoleId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId1");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Article", b =>
                {
                    b.HasOne("GoodNewsAggregator.DAL.Core.Rss", "Rss")
                        .WithMany("Articles")
                        .HasForeignKey("RssId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rss");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Comment", b =>
                {
                    b.HasOne("GoodNewsAggregator.DAL.Core.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GoodNewsAggregator.DAL.Core.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.User", b =>
                {
                    b.HasOne("GoodNewsAggregator.DAL.Core.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId1");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Article", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.Rss", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("GoodNewsAggregator.DAL.Core.User", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
