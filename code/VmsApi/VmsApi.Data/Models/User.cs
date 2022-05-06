using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        //todo make use of employee numbers:
        //public EmployeeNumber EmployeeNumber { get; set; }
        //todo add a specific farm (the employer) to the user!
        //public Farm Farm{ get; set; }

        public List<AssignedTask> AssignedTasks { get; set; }
    }
}