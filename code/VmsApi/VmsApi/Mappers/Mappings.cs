using System.Diagnostics.CodeAnalysis;
using VmsApi.Data.Models;
using VmsApi.Mappers;

namespace VmsApi.Utils
{
    [ExcludeFromCodeCoverage]
    public class Mappings: IMappings
    {
        public MeasurePoint MapToMeasurePoint(PostMapperMeasurePoint mapperMeasurePoint)
        {
            return new MeasurePoint()
            {
                Date = mapperMeasurePoint.Date,
                Amount = mapperMeasurePoint.Amount,
                Weight = mapperMeasurePoint.Weight
            };
        }

        public FoodPurchase MapToFoodPurchase(PostMapperFoodPurchase mapperFoodPurchase)
        {
            return new FoodPurchase()
            {
                Date = mapperFoodPurchase.Date,
                Amount = mapperFoodPurchase.Amount
            };
        }
    }
}