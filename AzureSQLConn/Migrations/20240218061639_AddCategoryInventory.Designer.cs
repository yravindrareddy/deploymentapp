﻿// <auto-generated />
using AzureSQLConn.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AzureSQLConn.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20240218061639_AddCategoryInventory")]
    partial class AddCategoryInventory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AzureSQLConn.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Clothing and accessories",
                            Name = "Apparel"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Sporting goods",
                            Name = "Sports"
                        });
                });

            modelBuilder.Entity("AzureSQLConn.Entities.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Inventory");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ProductId = 1,
                            Quantity = 50
                        },
                        new
                        {
                            Id = 2,
                            ProductId = 2,
                            Quantity = 100
                        },
                        new
                        {
                            Id = 3,
                            ProductId = 3,
                            Quantity = 75
                        });
                });

            modelBuilder.Entity("AzureSQLConn.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AvailableStock")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AvailableStock = 50,
                            CategoryId = 1,
                            Description = "Comfortable running shoes",
                            Name = "Nike Running Shoes",
                            Price = 99.99m
                        },
                        new
                        {
                            Id = 2,
                            AvailableStock = 100,
                            CategoryId = 2,
                            Description = "High-quality soccer ball",
                            Name = "Adidas Soccer Ball",
                            Price = 24.99m
                        },
                        new
                        {
                            Id = 3,
                            AvailableStock = 75,
                            CategoryId = 1,
                            Description = "Moisture-wicking workout shirt",
                            Name = "Under Armour T-Shirt",
                            Price = 29.99m
                        });
                });

            modelBuilder.Entity("AzureSQLConn.Entities.Inventory", b =>
                {
                    b.HasOne("AzureSQLConn.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("AzureSQLConn.Entities.Product", b =>
                {
                    b.HasOne("AzureSQLConn.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("AzureSQLConn.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
