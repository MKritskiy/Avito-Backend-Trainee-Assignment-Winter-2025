﻿// <auto-generated />
using System;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastucture.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250215021014_AddNullableCoinTransfer")]
    partial class AddNullableCoinTransfer
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.CoinTransfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int?>("FromUserId")
                        .HasColumnType("integer");

                    b.Property<int?>("ToUserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("CoinTransfers");
                });

            modelBuilder.Entity("Domain.Entities.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("Domain.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Items");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "t-shirt",
                            Price = 80
                        },
                        new
                        {
                            Id = 2,
                            Name = "cup",
                            Price = 20
                        },
                        new
                        {
                            Id = 3,
                            Name = "book",
                            Price = 50
                        },
                        new
                        {
                            Id = 4,
                            Name = "pen",
                            Price = 10
                        },
                        new
                        {
                            Id = 5,
                            Name = "powerbank",
                            Price = 200
                        },
                        new
                        {
                            Id = 6,
                            Name = "hoody",
                            Price = 300
                        },
                        new
                        {
                            Id = 7,
                            Name = "umbrella",
                            Price = 200
                        },
                        new
                        {
                            Id = 8,
                            Name = "socks",
                            Price = 10
                        },
                        new
                        {
                            Id = 9,
                            Name = "wallet",
                            Price = 50
                        },
                        new
                        {
                            Id = 10,
                            Name = "pink-hoody",
                            Price = 500
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Coins")
                        .HasColumnType("integer");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.CoinTransfer", b =>
                {
                    b.HasOne("Domain.Entities.User", "FromUser")
                        .WithMany("SentCoinTransfers")
                        .HasForeignKey("FromUserId");

                    b.HasOne("Domain.Entities.User", "ToUser")
                        .WithMany("ReceivedCoinTransfers")
                        .HasForeignKey("ToUserId");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("Domain.Entities.InventoryItem", b =>
                {
                    b.HasOne("Domain.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("InventoryItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("InventoryItems");

                    b.Navigation("ReceivedCoinTransfers");

                    b.Navigation("SentCoinTransfers");
                });
#pragma warning restore 612, 618
        }
    }
}
