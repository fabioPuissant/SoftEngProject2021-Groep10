using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class MeasurePoint
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public float Weight { get; set; }
        public int Amount { get; set; }
        public PigGroup PigGroup { get; set; }
        
    }
}