using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class PigGroup
    {
        public Guid Id { get; set; }
        public DateTime BirthDate { get; set; }
        public string GroupNumber { get; set; }
        public ICollection<FoodPurchase> FoodPurchases { get; set; }
        public ICollection<MeasurePoint> MeasurePoints { get; set; }
    }
}