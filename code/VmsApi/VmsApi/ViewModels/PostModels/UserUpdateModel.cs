using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VmsApi.CustomAttributes;
using VmsApi.Data.Utils;

namespace VmsApi.ViewModels.PostModels
{
    [ExcludeFromCodeCoverage]
    
    public class UserUpdateModel
    {
        /*
        [AllowedUserRoles(allowedValues: new string[]{"ADMINISTRATOR", "ACCOUNTANT", "AGENDA", "CEO", "MANAGER", "NUTRITION", "WEGER", "EMPLOYEE"})]
        public List<string> Roles { get; set; }
        */
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
