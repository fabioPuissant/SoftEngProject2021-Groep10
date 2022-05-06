using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Mappers
{
    [ExcludeFromCodeCoverage]
    public class PostMapperMeasurePoint
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float Weight { get; set; }
        public int Amount { get; set; }
        [Required]
        public string GroupNumber { get; set; }
    }
}