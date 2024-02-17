﻿// <auto-generated />
using System;
using Mango.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mango.Services.CouponAPI.Models.Coupon", b =>
                {
                    b.Property<int>("CouponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CouponId"));

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MinAmount")
                        .HasColumnType("integer");

                    b.HasKey("CouponId");

                    b.ToTable("coupons");

                    b.HasData(
                        new
                        {
                            CouponId = 1,
                            CouponCode = "10OFF",
                            DiscountAmount = 10.0,
                            LastUpdate = new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3042),
                            MinAmount = 20
                        },
                        new
                        {
                            CouponId = 2,
                            CouponCode = "20OFF",
                            DiscountAmount = 10.0,
                            LastUpdate = new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3072),
                            MinAmount = 40
                        },
                        new
                        {
                            CouponId = 3,
                            CouponCode = "30OFF",
                            DiscountAmount = 10.0,
                            LastUpdate = new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3082),
                            MinAmount = 50
                        });
                });
#pragma warning restore 612, 618
        }
    }
}