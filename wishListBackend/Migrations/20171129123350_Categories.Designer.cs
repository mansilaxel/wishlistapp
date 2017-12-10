using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using wishListBackend.Models;

namespace wishListBackend.Migrations
{
    [DbContext(typeof(wishListContext))]
    [Migration("20171129123350_Categories")]
    partial class Categories
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("wishListBackend.ParticipantOnWishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("UserId");

                    b.Property<int>("WishListId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WishListId");

                    b.ToTable("ParticipantOnWishList");
                });

            modelBuilder.Entity("wishListBackend.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("SecondName");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("wishListBackend.Wish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsBought");

                    b.Property<string>("Picture");

                    b.Property<string>("Title");

                    b.Property<int?>("WishCategoryId");

                    b.Property<int?>("WishListId");

                    b.HasKey("Id");

                    b.HasIndex("WishCategoryId");

                    b.HasIndex("WishListId");

                    b.ToTable("Wish");
                });

            modelBuilder.Entity("wishListBackend.WishCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Naam");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WishCategory");
                });

            modelBuilder.Entity("wishListBackend.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WishList");
                });

            modelBuilder.Entity("wishListBackend.ParticipantOnWishList", b =>
                {
                    b.HasOne("wishListBackend.User", "User")
                        .WithMany("ParticipantOnWishLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("wishListBackend.WishList", "WishList")
                        .WithMany("ParticipantOnWishLists")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("wishListBackend.Wish", b =>
                {
                    b.HasOne("wishListBackend.WishCategory", "WishCategory")
                        .WithMany()
                        .HasForeignKey("WishCategoryId");

                    b.HasOne("wishListBackend.WishList")
                        .WithMany("Wishes")
                        .HasForeignKey("WishListId");
                });

            modelBuilder.Entity("wishListBackend.WishCategory", b =>
                {
                    b.HasOne("wishListBackend.User")
                        .WithMany("MyWishCategories")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("wishListBackend.WishList", b =>
                {
                    b.HasOne("wishListBackend.User")
                        .WithMany("MyWishLists")
                        .HasForeignKey("UserId");
                });
        }
    }
}
