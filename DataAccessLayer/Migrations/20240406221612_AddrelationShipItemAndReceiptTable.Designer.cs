﻿// <auto-generated />
using System;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240406221612_AddrelationShipItemAndReceiptTable")]
    partial class AddrelationShipItemAndReceiptTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusinessLayer.Models.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("BusinessLayer.Models.DiscountItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<int?>("ItemDataId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DiscountId");

                    b.HasIndex("ItemDataId");

                    b.ToTable("DiscountItems");
                });

            modelBuilder.Entity("BusinessLayer.Models.ItemData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ReceiptId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReceiptId");

                    b.ToTable("itemDatas");
                });

            modelBuilder.Entity("BusinessLayer.Models.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalDiscount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalSales")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalTax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("Id");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("BusinessLayer.Models.Tax", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TaxType")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("Id");

                    b.ToTable("Taxes");
                });

            modelBuilder.Entity("BusinessLayer.Models.TaxItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ItemDataId")
                        .HasColumnType("int");

                    b.Property<int?>("TaxId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemDataId");

                    b.HasIndex("TaxId");

                    b.ToTable("TaxItems");
                });

            modelBuilder.Entity("BusinessLayer.Models.DiscountItem", b =>
                {
                    b.HasOne("BusinessLayer.Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BusinessLayer.Models.ItemData", "ItemData")
                        .WithMany("DiscountItems")
                        .HasForeignKey("ItemDataId");

                    b.Navigation("Discount");

                    b.Navigation("ItemData");
                });

            modelBuilder.Entity("BusinessLayer.Models.ItemData", b =>
                {
                    b.HasOne("BusinessLayer.Models.Receipt", null)
                        .WithMany("ItemData")
                        .HasForeignKey("ReceiptId");
                });

            modelBuilder.Entity("BusinessLayer.Models.TaxItem", b =>
                {
                    b.HasOne("BusinessLayer.Models.ItemData", "ItemData")
                        .WithMany("TaxItems")
                        .HasForeignKey("ItemDataId");

                    b.HasOne("BusinessLayer.Models.Tax", "Tax")
                        .WithMany()
                        .HasForeignKey("TaxId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ItemData");

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("BusinessLayer.Models.ItemData", b =>
                {
                    b.Navigation("DiscountItems");

                    b.Navigation("TaxItems");
                });

            modelBuilder.Entity("BusinessLayer.Models.Receipt", b =>
                {
                    b.Navigation("ItemData");
                });
#pragma warning restore 612, 618
        }
    }
}
