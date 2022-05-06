using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VmsApi.Data.Models;

namespace VmsApi.ViewModels.GetModels
{
    public class UserGetModel
    {
        public IList<AssignedTask> AssignedTasks { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public IList<string> Roles{ get; set; }
    }
}
