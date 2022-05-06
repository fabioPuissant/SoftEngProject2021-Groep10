using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VmsApi.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class AssignedTask
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }
        public String UserId { get; set; }
        public User User { get; set; }
    }
}
