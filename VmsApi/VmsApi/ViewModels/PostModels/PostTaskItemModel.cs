using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace VmsApi.ViewModels.PostModels
{
    [ExcludeFromCodeCoverage]
    public class PostTaskItemModel
    {
        [Required]
        public String TaskTitle { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public Boolean Archived { get; set; }
        public bool Completed { get; set; }
        [Required]
        public int RepeatingIntervalDays { get; set; }
    }
}
