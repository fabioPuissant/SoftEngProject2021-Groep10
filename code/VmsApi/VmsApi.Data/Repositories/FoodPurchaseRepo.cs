using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VmsApi.Data.DataDbContext;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories
{
    public class FoodPurchaseRepo : IFoodPurchasesRepo
    {
        private readonly VmsDbContext _context;

        public FoodPurchaseRepo(VmsDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodPurchase>> GetAll()
        {
            return await _context.FoodPurchases.ToListAsync();
        }

        public async Task<FoodPurchase> GetByIdAsync(Guid id)
        {
            return await _context.FoodPurchases
                .Include(f => f.PigGroup)
                .FirstOrDefaultAsync(foodPurchase => foodPurchase.Id == id);
        }

        public async Task<FoodPurchase> CreateAsync(string groupNumber, FoodPurchase purchase)
        {
            var group = await _context.PigGroups.Include(g => g.FoodPurchases).SingleAsync(pigGroup => pigGroup.GroupNumber == groupNumber);
            group.FoodPurchases.Add(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<FoodPurchase> UpdateAsync(Guid id, FoodPurchase updatedItem)
        {
            var found = await GetByIdAsync(id);
            if (found == null)
            {
                throw new IdException($"No FoodPurchaseFound with id {id}");
            }

            found.Date = updatedItem.Date;
            found.Amount = updatedItem.Amount;
            
            _context.Entry(found).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return found;
        }

        public async Task DeleteAsync(FoodPurchase item)
        {
            var mp = _context.FoodPurchases.Where(m => m.Id == item.Id).FirstOrDefault();
            mp.PigGroup = null;
            _context.Entry(mp).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
