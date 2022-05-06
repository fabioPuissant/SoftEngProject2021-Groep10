using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VmsApi.Data.Models;

namespace VmsApi.Data.DataDbContext
{
    [ExcludeFromCodeCoverage]
    public class VmsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<IdentityUserRole<string>> Roles { get; set; }
        public DbSet<FoodPurchase> FoodPurchases { get; set; }
        public DbSet<PigGroup> PigGroups { get; set; }
        public DbSet<MeasurePoint> MeasurePoints { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        public VmsDbContext(DbContextOptions<VmsDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });

            var defaultUser = new User()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "xxxx@example.com",
                NormalizedEmail = "XXXX@EXAMPLE.COM",
                UserName = "Owner",
                NormalizedUserName = "OWNER",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "96bd606a-2d23-4cc7-a929-e380d181d0e6",
                Id = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                PasswordHash = "AQAAAAEAACcQAAAAEGpaVsQ7M5pomvpiM9HvM4/i8tDIyNp2hU9LY8dCggz4FdHwnqhRFwyW5/zNJnN4aw==",
                ConcurrencyStamp = "687a9571-1098-4e18-bedc-f7075207a573",
            };

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                defaultUser
             );
            
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>()
                {
                    RoleId = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00",
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "36c604a2-1f4e-4552-8741-74140540679b",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "8cac7619-270a-4591-add3-4f8b983ed286",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "a5d32981-f064-4277-b061-f4dc6eba9ef5",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "b645b107-532b-4c9a-a1f0-d807dbac63b0",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "b888e790-e6f8-49ce-b5b5-bab5ee97593a",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "c54eb66f-4a09-4320-b9eb-0a9c6ae1d39a",
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00"
                },
            });

            modelBuilder.Entity<AssignedTask>()
                .HasOne(x => x.User)
                .WithMany(x => x.AssignedTasks)
                .HasForeignKey((x => x.UserId));
            modelBuilder.Entity<AssignedTask>()
                .HasOne(x => x.TaskItem)
                .WithMany(x => x.AssignedTasks)
                .HasForeignKey((x => x.TaskItemId));

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"),
                    TaskTitle = "Varkens voederen",
                    Description = "Het voer ligt in de schuur.",
                    StartDate = new DateTime(2021, 05, 20),
                    DueDate = new DateTime(2021, 04, 1),
                    Completed = false,
                    Archived = false,
                    RepeatingIntervalDays = 0,
                },

                new TaskItem
                {
                    Id = new Guid("626ea8c4-2ea4-45c6-8c08-c522eaaaaaaa"),
                    TaskTitle = "Varkens vaccineren tegen Corona",
                    Description = "Anders kunnen ze niet feesten.",
                    StartDate = new DateTime(2021, 05, 22),
                    DueDate = new DateTime(2021, 04, 1),
                    Completed = true,
                    Archived = false,
                    RepeatingIntervalDays = 1,
                }
            );

            modelBuilder.Entity<AssignedTask>().HasData(
                new AssignedTask()
                {
                    Id = new Guid("626ea8c4-3333-45c6-8c08-c522eca14b0a"),
                    TaskItemId = new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"), //TaskItem: Varkens voederen
                    UserId = "626ea8c4-2ea4-45c6-8c08-c522eca14b00" //User: FabioXXX
                }
            );


            modelBuilder.Entity<PigGroup>()
                .HasMany(p => p.MeasurePoints)
                .WithOne(m => m.PigGroup);

            modelBuilder.Entity<PigGroup>()
                .HasMany(p => p.FoodPurchases)
                .WithOne(f => f.PigGroup);

            modelBuilder.Entity<PigGroup>().Property(p => p.GroupNumber).IsRequired();
            modelBuilder.Entity<PigGroup>().Property(p => p.BirthDate).IsRequired();

            modelBuilder.Entity<PigGroup>(entity =>
            {
                entity.HasIndex(p => p.GroupNumber).IsUnique();
            });
        }
    }
}