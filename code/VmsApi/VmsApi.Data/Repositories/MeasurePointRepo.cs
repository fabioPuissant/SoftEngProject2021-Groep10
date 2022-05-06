using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VmsApi.Data.DataDbContext;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories
{
    public class MeasurePointRepo : IMeasurePointRepo
    {
        private readonly VmsDbContext _context;

        public MeasurePointRepo(VmsDbContext context)
        {
            _context = context;
        }

        public async Task<List<MeasurePoint>> GetAll()
        {

            return await _context.MeasurePoints.ToListAsync();
        }

        public async Task<MeasurePoint> GetById(Guid id)
        {
            return await _context.MeasurePoints
                .Include(f => f.PigGroup)
                .FirstOrDefaultAsync(pigGroup => pigGroup.Id == id);
        }

        public async Task<MeasurePoint> Create(string groupNumber, MeasurePoint point)
        {
            var group = await _context.PigGroups.Include(g => g.MeasurePoints).SingleAsync(pigGroup => pigGroup.GroupNumber == groupNumber);
            group.MeasurePoints.Add(point);
            await _context.SaveChangesAsync();

            return point;
        }

        public async Task<MeasurePoint> Update(Guid id, MeasurePoint updatedItem)
        {
            var found = await GetById(id);
            if (found == null)
            {
                throw new IdException($"No MeasurePoint found with id of {id}");
            }

            found.Date = updatedItem.Date;
            found.Amount = updatedItem.Amount;
            found.Weight = updatedItem.Weight;
            
            _context.Entry(found).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return found;
        }

        public async Task DeleteAsync(MeasurePoint item)
        {
            var mp = _context.MeasurePoints.Where(m => m.Id == item.Id).FirstOrDefault();
            mp.PigGroup = null;
            _context.Entry(mp).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
