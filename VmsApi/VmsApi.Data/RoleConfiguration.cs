using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VmsApi.Data.Utils;

namespace VmsApi.Data
{
    [ExcludeFromCodeCoverage]
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3",
                    Name = AppRolesDict.ApplicationRoles["ADMINISTRATOR"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["ADMINISTRATOR"].NormalizedName
                },
                new IdentityRole
                {
                    Id = "36c604a2-1f4e-4552-8741-74140540679b",
                    Name = AppRolesDict.ApplicationRoles["ACCOUNTANT"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["ACCOUNTANT"].NormalizedName,
                },
                new IdentityRole
                {
                    Id = "8cac7619-270a-4591-add3-4f8b983ed286",
                    Name = AppRolesDict.ApplicationRoles["AGENDA"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["AGENDA"].NormalizedName
                },
                new IdentityRole
                {
                    Name = AppRolesDict.ApplicationRoles["CEO"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["CEO"].NormalizedName
                },
                new IdentityRole
                {
                    Id = "a5d32981 - f064 - 4277 - b061 - f4dc6eba9ef5",
                    Name = AppRolesDict.ApplicationRoles["MANAGER"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["MANAGER"].NormalizedName,
                },
                new IdentityRole
                {
                    Id = "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f",
                    Name = AppRolesDict.ApplicationRoles["NUTRITION"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["NUTRITION"].NormalizedName,
                },
                new IdentityRole
                {
                    Id = "b645b107-532b-4c9a-a1f0-d807dbac63b0",
                    Name = AppRolesDict.ApplicationRoles["WEGER"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["WEGER"].NormalizedName
                },
                new IdentityRole
                {
                    Id = "b888e790-e6f8-49ce-b5b5-bab5ee97593a",
                    Name = AppRolesDict.ApplicationRoles["EMPLOYEE"].Name,
                    NormalizedName = AppRolesDict.ApplicationRoles["EMPLOYEE"].NormalizedName
                }
            );
            
        }
    }
}