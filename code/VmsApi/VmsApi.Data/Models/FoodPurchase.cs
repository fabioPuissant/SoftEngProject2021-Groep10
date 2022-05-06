using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace VmsApi.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class FoodPurchase
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float Amount { get; set; }
        public PigGroup PigGroup { get; set; }
    }
}
