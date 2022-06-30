﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VmsApi.Data.DataDbContext;

namespace VmsApi.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(VmsDbContext))]
    partial class VmsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityRole");

                    b.HasData(
                        new
                        {
                            Id = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3",
                            ConcurrencyStamp = "d8e8e1bd-0910-4400-be4d-a640c4647be5",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "36c604a2-1f4e-4552-8741-74140540679b",
                            ConcurrencyStamp = "2cede2d1-520a-4201-8f60-5f7faa410236",
                            Name = "Administratief_medewerker",
                            NormalizedName = "ACCOUNTANT"
                        },
                        new
                        {
                            Id = "8cac7619-270a-4591-add3-4f8b983ed286",
                            ConcurrencyStamp = "d64e96f0-9d95-436d-9bd1-0f2ed21a571e",
                            Name = "Agendabeheerder",
                            NormalizedName = "AGENDA"
                        },
                        new
                        {
                            Id = "0c9108de-3cf3-41c1-8758-b40787797941",
                            ConcurrencyStamp = "a8184dae-3c65-4517-8a11-eb98f6f799e0",
                            Name = "Chief_Executive_Officer",
                            NormalizedName = "CEO"
                        },
                        new
                        {
                            Id = "a5d32981 - f064 - 4277 - b061 - f4dc6eba9ef5",
                            ConcurrencyStamp = "0a1a5034-62a7-4ee7-94de-11208b65cbdc",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f",
                            ConcurrencyStamp = "cac2110d-f92d-4a2b-85c6-12081d6ba36c",
                            Name = "Voedingsbeheerder",
                            NormalizedName = "NUTRITION"
                        },
                        new
                        {
                            Id = "b645b107-532b-4c9a-a1f0-d807dbac63b0",
                            ConcurrencyStamp = "a5dbe2df-5d7f-4bfc-99f5-29d9630e026a",
                            Name = "Weger",
                            NormalizedName = "WEGER"
                        },
                        new
                        {
                            Id = "b888e790-e6f8-49ce-b5b5-bab5ee97593a",
                            ConcurrencyStamp = "e7438c01-1541-4033-939e-a55f0fb631f7",
                            Name = "Werknemer",
                            NormalizedName = "EMPLOYEE"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "36c604a2-1f4e-4552-8741-74140540679b"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "8cac7619-270a-4591-add3-4f8b983ed286"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "a5d32981-f064-4277-b061-f4dc6eba9ef5"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "b645b107-532b-4c9a-a1f0-d807dbac63b0"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "b888e790-e6f8-49ce-b5b5-bab5ee97593a"
                        },
                        new
                        {
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            RoleId = "c54eb66f-4a09-4320-b9eb-0a9c6ae1d39a"
                        });
                });

            modelBuilder.Entity("VmsApi.Data.Models.AssignedTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TaskItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TaskItemId");

                    b.HasIndex("UserId");

                    b.ToTable("AssignedTask");

                    b.HasData(
                        new
                        {
                            Id = new Guid("626ea8c4-3333-45c6-8c08-c522eca14b0a"),
                            TaskItemId = new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"),
                            UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                        });
                });

            modelBuilder.Entity("VmsApi.Data.Models.FoodPurchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PigGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PigGroupId");

                    b.ToTable("FoodPurchases");
                });

            modelBuilder.Entity("VmsApi.Data.Models.MeasurePoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PigGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("PigGroupId");

                    b.ToTable("MeasurePoints");
                });

            modelBuilder.Entity("VmsApi.Data.Models.PigGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("GroupNumber")
                        .IsUnique();

                    b.ToTable("PigGroups");
                });

            modelBuilder.Entity("VmsApi.Data.Models.TaskItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatorId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RepeatingIntervalDays")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId1");

                    b.ToTable("TaskItems");

                    b.HasData(
                        new
                        {
                            Id = new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"),
                            Completed = false,
                            CreatorId = new Guid("00000000-0000-0000-0000-000000000000"),
                            Description = "Het voer ligt in de schuur.",
                            DueDate = new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RepeatingIntervalDays = 0,
                            StartDate = new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TaskTitle = "Varkens voederen"
                        },
                        new
                        {
                            Id = new Guid("626ea8c4-2ea4-45c6-8c08-c522eaaaaaaa"),
                            Completed = true,
                            CreatorId = new Guid("00000000-0000-0000-0000-000000000000"),
                            Description = "Anders kunnen ze niet feesten.",
                            DueDate = new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RepeatingIntervalDays = 1,
                            StartDate = new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TaskTitle = "Varkens vaccineren tegen Corona"
                        });
                });

            modelBuilder.Entity("VmsApi.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "687a9571-1098-4e18-bedc-f7075207a573",
                            Email = "xxxx@example.com",
                            EmailConfirmed = true,
                            FirstName = "John",
                            LastName = "Doe",
                            LockoutEnabled = false,
                            NormalizedEmail = "XXXX@EXAMPLE.COM",
                            NormalizedUserName = "OWNER",
                            PasswordHash = "AQAAAAEAACcQAAAAEGpaVsQ7M5pomvpiM9HvM4/i8tDIyNp2hU9LY8dCggz4FdHwnqhRFwyW5/zNJnN4aw==",
                            PhoneNumber = "+111111111111",
                            PhoneNumberConfirmed = true,
                            SecurityStamp = "96bd606a-2d23-4cc7-a929-e380d181d0e6",
                            TwoFactorEnabled = false,
                            UserName = "Owner"
                        });
                });

            modelBuilder.Entity("VmsApi.Data.Models.AssignedTask", b =>
                {
                    b.HasOne("VmsApi.Data.Models.TaskItem", "TaskItem")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("TaskItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VmsApi.Data.Models.User", "User")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("VmsApi.Data.Models.FoodPurchase", b =>
                {
                    b.HasOne("VmsApi.Data.Models.PigGroup", "PigGroup")
                        .WithMany("FoodPurchases")
                        .HasForeignKey("PigGroupId");
                });

            modelBuilder.Entity("VmsApi.Data.Models.MeasurePoint", b =>
                {
                    b.HasOne("VmsApi.Data.Models.PigGroup", "PigGroup")
                        .WithMany("MeasurePoints")
                        .HasForeignKey("PigGroupId");
                });

            modelBuilder.Entity("VmsApi.Data.Models.TaskItem", b =>
                {
                    b.HasOne("VmsApi.Data.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId1");
                });
#pragma warning restore 612, 618
        }
    }
}