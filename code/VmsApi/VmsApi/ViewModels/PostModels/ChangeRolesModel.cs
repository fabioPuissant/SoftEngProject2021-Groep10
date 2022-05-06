using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using VmsApi.CustomAttributes;

namespace VmsApi.ViewModels.PostModels
{
    [ExcludeFromCodeCoverage]
    public class ChangeRolesModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
      //  [AllowedUserRoles(allowedValues: new [] { "ADMINISTRATOR", "ACCOUNTANT", "AGENDA", "CEO", "MANAGER", "NUTRITION", "WEGER", "EMPLOYEE" })]
        public List<string> Roles { get; set; }

    }
}
