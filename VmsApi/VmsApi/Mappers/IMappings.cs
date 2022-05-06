using VmsApi.Data.Models;

namespace VmsApi.Mappers
{
    public interface IMappings
    {
        MeasurePoint MapToMeasurePoint(PostMapperMeasurePoint mapperMeasurePoint);
        FoodPurchase MapToFoodPurchase(PostMapperFoodPurchase mapperFoodPurchase);
    }
}