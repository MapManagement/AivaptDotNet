﻿// <auto-generated />
using System;
using AivaptDotNet.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AivaptDotNet.Migrations
{
    [DbContext(typeof(BotDbContext))]
    [Migration("20230201211202_AddCoordinateDescription")]
    partial class AddCoordinateDescription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AivaptDotNet.Services.Database.Guild", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("OwnerId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.ToTable("guild");
                });

            modelBuilder.Entity("AivaptDotNet.Services.Database.Models.McCoordinates", b =>
                {
                    b.Property<int>("CoordinatesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<ulong>("SubmitterId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("X")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("Y")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("Z")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("CoordinatesId");

                    b.ToTable("mc_coordinates");
                });

            modelBuilder.Entity("AivaptDotNet.Services.Database.Models.McLocation", b =>
                {
                    b.Property<uint>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("LocationId");

                    b.ToTable("mc_location");
                });

            modelBuilder.Entity("AivaptDotNet.Services.Database.Models.Quote", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.ToTable("quote");
                });

            modelBuilder.Entity("AivaptDotNet.Services.Database.Models.Role", b =>
                {
                    b.Property<ulong>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<bool>("ModPermissions")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("RoleId");

                    b.ToTable("role");
                });

            modelBuilder.Entity("AivaptDotNet.Services.Database.Models.SimpleCommand", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("#1ABC9C");

                    b.Property<ulong>("CreatorId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.HasKey("Name");

                    b.ToTable("simple_command");
                });

            modelBuilder.Entity("McCoordinatesMcLocation", b =>
                {
                    b.Property<int>("LinkedMcCoordinatesCoordinatesId")
                        .HasColumnType("int");

                    b.Property<uint>("LocationsLocationId")
                        .HasColumnType("int unsigned");

                    b.HasKey("LinkedMcCoordinatesCoordinatesId", "LocationsLocationId");

                    b.HasIndex("LocationsLocationId");

                    b.ToTable("McCoordinatesMcLocation");
                });

            modelBuilder.Entity("McCoordinatesMcLocation", b =>
                {
                    b.HasOne("AivaptDotNet.Services.Database.Models.McCoordinates", null)
                        .WithMany()
                        .HasForeignKey("LinkedMcCoordinatesCoordinatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AivaptDotNet.Services.Database.Models.McLocation", null)
                        .WithMany()
                        .HasForeignKey("LocationsLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
