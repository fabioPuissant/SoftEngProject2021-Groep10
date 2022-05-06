using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Mappers
{
    [ExcludeFromCodeCoverage]
    public class PostMapperFoodPurchase
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public string GroupNumber { get; set; }
    }
}