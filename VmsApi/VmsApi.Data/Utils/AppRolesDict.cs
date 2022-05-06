using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Utils
{
using System;
    [ExcludeFromCodeCoverage]
    public class AppRolesDict
    {
        static AppRolesDict()
        {
            ApplicationRoles = new Dictionary<string, AppRole>();

            ApplicationRoles.Add("ADMINISTRATOR", new AppRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );

            ApplicationRoles.Add("ACCOUNTANT", new AppRole
            {
                Name = "Administratief_medewerker",
                NormalizedName = "ACCOUNTANT"
            });

            ApplicationRoles.Add("AGENDA", new AppRole
            {
                Name = "Agendabeheerder",
                NormalizedName = "AGENDA"
            });

            ApplicationRoles.Add("CEO", new AppRole
            {
                Name = "Chief_Executive_Officer",
                NormalizedName = "CEO"
            });

            ApplicationRoles.Add("MANAGER", new AppRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            });

            ApplicationRoles.Add("NUTRITION", new AppRole
            {
                Name = "Voedingsbeheerder",
                NormalizedName = "NUTRITION"
            });

            ApplicationRoles.Add("WEGER", new AppRole
            {
                Name = "Weger",
                NormalizedName = "WEGER"
            });

            ApplicationRoles.Add("EMPLOYEE", new AppRole
            {
                Name = "Werknemer",
                NormalizedName = "EMPLOYEE"
            });
        }

        public static Dictionary<string, AppRole> ApplicationRoles { get; private set; }
    }
}
