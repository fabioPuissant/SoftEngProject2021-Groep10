using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VmsApi.Data.Models
{
    [ExcludeFromCodeCoverage]

    public class TaskItem
    {
        [Key]
        public Guid Id { get; set; }
        public String TaskTitle { get; set; }
        public String Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public int RepeatingIntervalDays { get; set; }
        public bool Archived { get; set; }
        public List<AssignedTask> AssignedTasks { get; set; }
    }
}
