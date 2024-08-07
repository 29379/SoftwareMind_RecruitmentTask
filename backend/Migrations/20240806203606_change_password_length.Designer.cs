﻿// <auto-generated />
using System;
using HotDeskBookingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotDeskBookingSystem.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240806203606_change_password_length")]
    partial class change_password_length
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.AppUser", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HashPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<string>("BookingStatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("DeskId")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("endTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("startTime")
                        .HasColumnType("datetime2");

                    b.HasKey("BookingId");

                    b.HasIndex("BookingStatusName");

                    b.HasIndex("DeskId");

                    b.HasIndex("UserEmail");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.BookingStatus", b =>
                {
                    b.Property<string>("StatusName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StatusName");

                    b.ToTable("BookingStatuses");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Desk", b =>
                {
                    b.Property<int>("DeskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeskId"));

                    b.Property<int>("OfficeFloorId")
                        .HasColumnType("int");

                    b.HasKey("DeskId");

                    b.HasIndex("OfficeFloorId");

                    b.ToTable("Desks");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Office", b =>
                {
                    b.Property<int>("OfficeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OfficeId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TotalDesks")
                        .HasColumnType("int");

                    b.Property<int>("TotalFloors")
                        .HasColumnType("int");

                    b.HasKey("OfficeId");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.OfficeFloor", b =>
                {
                    b.Property<int>("OfficeFloorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OfficeFloorId"));

                    b.Property<int>("FloorNumber")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfDesks")
                        .HasColumnType("int");

                    b.Property<int>("OfficeId")
                        .HasColumnType("int");

                    b.HasKey("OfficeFloorId");

                    b.HasIndex("OfficeId");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Role", b =>
                {
                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleName");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.UserRole", b =>
                {
                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnOrder(0);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.HasKey("UserEmail", "RoleName");

                    b.HasIndex("RoleName");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Booking", b =>
                {
                    b.HasOne("HotDeskBookingSystem.Data.Models.BookingStatus", "BookingStatus")
                        .WithMany("Bookings")
                        .HasForeignKey("BookingStatusName")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HotDeskBookingSystem.Data.Models.Desk", "Desk")
                        .WithMany("Bookings")
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotDeskBookingSystem.Data.Models.AppUser", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BookingStatus");

                    b.Navigation("Desk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Desk", b =>
                {
                    b.HasOne("HotDeskBookingSystem.Data.Models.OfficeFloor", "OfficeFloor")
                        .WithMany("Desks")
                        .HasForeignKey("OfficeFloorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("OfficeFloor");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.OfficeFloor", b =>
                {
                    b.HasOne("HotDeskBookingSystem.Data.Models.Office", "Office")
                        .WithMany("OfficeFloors")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Office");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.UserRole", b =>
                {
                    b.HasOne("HotDeskBookingSystem.Data.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotDeskBookingSystem.Data.Models.AppUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.AppUser", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.BookingStatus", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Desk", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Office", b =>
                {
                    b.Navigation("OfficeFloors");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.OfficeFloor", b =>
                {
                    b.Navigation("Desks");
                });

            modelBuilder.Entity("HotDeskBookingSystem.Data.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
