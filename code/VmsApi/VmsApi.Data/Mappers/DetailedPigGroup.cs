using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Mappers
{
    [ExcludeFromCodeCoverage]
    public class DetailedPigGroup
    {
        public Guid Id { get; set; }
        public string GroupNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime WeightDate { get; set; }
        public int TotalPigs { get; set; }
        public float TotalWeight { get; set; }
        public float AverageWeight { get; set; }
    }
}