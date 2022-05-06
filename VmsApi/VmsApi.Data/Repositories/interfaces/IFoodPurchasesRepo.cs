using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories
{
    public interface IFoodPurchasesRepo
    {
        Task<List<FoodPurchase>> GetAll();
        Task<FoodPurchase> GetByIdAsync(Guid id);
        Task<FoodPurchase> CreateAsync(string groupNumber, FoodPurchase purchase);
        Task<FoodPurchase> UpdateAsync(Guid id, FoodPurchase updatedItem);
        Task DeleteAsync(FoodPurchase item);
    }
}
