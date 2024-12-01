﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MonWebApp.Migrations
{
    [DbContext(typeof(WalletContext))]
    [Migration("20241126163422_AddQuantityToAsset")]
    partial class AddQuantityToAsset
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MonWebApp.Models.Asset", b =>
                {
                    b.Property<int>("AssetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("AssetId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("MonWebApp.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MonWebApp.Models.UserAsset", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("AssetId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("UserAssetId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "AssetId");

                    b.HasIndex("AssetId");

                    b.ToTable("UserAssets");
                });

            modelBuilder.Entity("MonWebApp.Models.UserAsset", b =>
                {
                    b.HasOne("MonWebApp.Models.Asset", "Asset")
                        .WithMany("UserAssets")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MonWebApp.Models.User", "User")
                        .WithMany("UserAssets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MonWebApp.Models.Asset", b =>
                {
                    b.Navigation("UserAssets");
                });

            modelBuilder.Entity("MonWebApp.Models.User", b =>
                {
                    b.Navigation("UserAssets");
                });
#pragma warning restore 612, 618
        }
    }
}
