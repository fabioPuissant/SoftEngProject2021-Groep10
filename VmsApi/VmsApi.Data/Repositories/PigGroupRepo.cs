using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VmsApi.Data.DataDbContext;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Mappers;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories.interfaces;

namespace VmsApi.Data.Repositories
{
    public class PigGroupRepo : IPigGroupRepo
    {
        private readonly VmsDbContext _context;

        public PigGroupRepo(VmsDbContext context)
        {
            _context = context;
        }

        // #biggen totaal gewicht gemideld gewicht/big bij de laatste include
        public async Task<List<DetailedPigGroup>> GetAll()
        {
            var groups = from g in _context.PigGroups
                select new DetailedPigGroup
                {
                    Id = g.Id,
                    GroupNumber = g.GroupNumber,
                    BirthDate = g.BirthDate,
                    WeightDate = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Date,
                    TotalPigs = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount,
                    TotalWeight = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight,
                    AverageWeight = (g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight /
                                     g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount),
                };

            return await groups.ToListAsync();
        }

        public async Task<PigGroup> GetById(Guid id)
        {
            return await _context.PigGroups
                .Include(p => p.MeasurePoints)
                .Include(p => p.FoodPurchases)
                .FirstOrDefaultAsync(pigGroup => pigGroup.Id == id);
        }

        // Alle groepen die geboren zijn tussen bepaalde dagen
        [ExcludeFromCodeCoverage]
        public async Task<List<DetailedPigGroup>> GetAllByDate(DateTime? startDateTime, DateTime? endDateTime)
        {
            if (startDateTime == null && endDateTime == null)
            {
                throw new DatePigGroupException("Start- and End date were null");
            }

            if (startDateTime.HasValue && endDateTime.HasValue && startDateTime.Value > endDateTime.Value)
            {
                throw new DatePigGroupException("Start Date must come before End Date");
            }

            if (startDateTime.HasValue && endDateTime.HasValue)
            {
                var groupsBetween = from g in _context.PigGroups
                    where g.BirthDate >= startDateTime.Value && g.BirthDate <= endDateTime.Value
                    select new DetailedPigGroup
                    {
                        Id = g.Id,
                        GroupNumber = g.GroupNumber,
                        BirthDate = g.BirthDate,
                        TotalPigs = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount,
                        TotalWeight = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight,
                        AverageWeight = (g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight /
                                         g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount),
                    };
                return await groupsBetween.ToListAsync();
            }

            if (startDateTime.HasValue)
            {
                var groupsFromStartDate = from g in _context.PigGroups
                    where g.BirthDate >= startDateTime.Value
                    select new DetailedPigGroup
                    {
                        Id = g.Id,
                        GroupNumber = g.GroupNumber,
                        BirthDate = g.BirthDate,
                        TotalPigs = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount,
                        TotalWeight = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight,
                        AverageWeight = (g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight /
                                         g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount),
                    };
                return await groupsFromStartDate.ToListAsync();
            }

            var groupsBeforeEndDate = from g in _context.PigGroups
                where g.BirthDate <= endDateTime.Value
                select new DetailedPigGroup
                {
                    Id = g.Id,
                    GroupNumber = g.GroupNumber,
                    BirthDate = g.BirthDate,
                    TotalPigs = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount,
                    TotalWeight = g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight,
                    AverageWeight = (g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Weight /
                                     g.MeasurePoints.OrderByDescending(m => m.Date).FirstOrDefault().Amount),
                };
            return await groupsBeforeEndDate.ToListAsync();
        }


        public int GetClosestPreviousDateTimeIndex(List<FoodPurchase> purchases, DateTime specificTime)
        {
            int ret = -1;
            var lowestDifference = TimeSpan.MaxValue;

            for (int i = 0; i< purchases.Count(); i++)
            {
                if (purchases[i].Date > specificTime)
                    continue;

                var difference = specificTime - purchases[i].Date;

                if (difference < lowestDifference)
                {
                    lowestDifference = difference;
                    ret = i;
                }
            }

            return ret;
        }
        public int GetClosestNextDateTimeIndex(List<FoodPurchase> purchases, DateTime specificTime)
        {
            int ret = -1;
            var lowestDifference = TimeSpan.MaxValue;

            for (int i = 0; i < purchases.Count(); i++)
            {
                if (purchases[i].Date < specificTime)
                    continue;

                var difference = purchases[i].Date - specificTime;

                if (difference < lowestDifference)
                {
                    lowestDifference = difference;
                    ret = i;
                }
            }

            return ret;
        }

        public async Task<Dictionary<int, float>> GetGrowthGroup(Guid id)
        {
            Dictionary<int, float> result = new Dictionary<int, float>();

            var group = await _context.PigGroups
                .Include(p => p.MeasurePoints)
                .Include(p => p.FoodPurchases)
                .FirstOrDefaultAsync(pigGroup => pigGroup.Id == id);

            var weights = group.MeasurePoints.OrderBy(m => m.Date).ToList();

            if (weights.Count() == 0)
            {
                return result;
            }

            var birthDate = weights[0].Date;

            for (int i = 1; i < weights.Count(); i++)
            {
                int prevAge = (weights[i-1].Date - birthDate).Days;
                int age = (weights[i].Date - birthDate).Days;
                int diffAge = age - prevAge;

                float prevWeight = weights[i - 1].Weight;
                float weight = weights[i].Weight ;
                float diffWeight = weight - prevWeight;

                float growth = diffWeight / diffAge;

                result.Add(age, growth);
            }

            return result;
        }

        public async Task<Dictionary<int, float>> GetFoodUseGroup(Guid id)
        {
            Dictionary<int, float> result = new Dictionary<int, float>();

            var group = await _context.PigGroups
                .Include(p => p.MeasurePoints)
                .Include(p => p.FoodPurchases)
                .FirstOrDefaultAsync(pigGroup => pigGroup.Id == id);

            var foodPurchases = group.FoodPurchases.OrderBy(f => f.Date).ToList();

            if (foodPurchases.Count() == 0)
            {
                return result;
            }

            var birthDate = foodPurchases[0].Date;

            for (int i = 1; i < foodPurchases.Count(); i++)
            {
                int prevAge = (foodPurchases[i - 1].Date - birthDate).Days;
                int age = (foodPurchases[i].Date - birthDate).Days;
                int diffAge = age - prevAge;

                float amountEaten = foodPurchases[i - 1].Amount;

                float foodUse = amountEaten / diffAge;

                result.Add(age, foodUse);
            }

            return result;
        }

        public async Task<Dictionary<int, float>> GetVCGroup(Guid id)
        {
            Dictionary<int, float> result = new Dictionary<int, float>();

            var group =  await _context.PigGroups
            .Include(p => p.MeasurePoints)
            .Include(p => p.FoodPurchases)
            .FirstOrDefaultAsync(pigGroup => pigGroup.Id == id);

            var weights = group.MeasurePoints.OrderBy(m => m.Date).ToList();
            var foodPurchases = group.FoodPurchases.OrderBy(f => f.Date).ToList();

            if (weights.Count() == 0 || foodPurchases.Count() == 0)
            {
                return result;
            }

            var birthDate = weights[0].Date;

            IList<int> ages = new List<int>();

            for (int i = 1; i < weights.Count(); i++)
            {

                int age = (weights[i].Date - birthDate).Days;

                // vind voer die het dichts bij datewx-1 ligt en kleiner of gelijk aan is.
                var dateWx1 = weights[i - 1].Date;
                var closestVoerDateWx1Index = GetClosestPreviousDateTimeIndex(foodPurchases, dateWx1);

                // bereken hoeveel hiervan gem per dag gegeten word.
                var voerx1 = foodPurchases[closestVoerDateWx1Index];
                DateTime foodEmptyDate;
                int diffVoerx1Dates;
                if (foodPurchases.Count() -1 >= closestVoerDateWx1Index + 1)
                {
                    foodEmptyDate = foodPurchases[closestVoerDateWx1Index + 1].Date;
                    diffVoerx1Dates = (foodEmptyDate - voerx1.Date).Days;
                }
                else
                {
                    foodEmptyDate = weights[weights.Count()-1].Date;
                    diffVoerx1Dates = (foodEmptyDate - voerx1.Date).Days;
                }

                //voerx1Next = foodPurchases[closestVoerDateWx1Index + 1];
                //diffVoerx1Dates = (voerx1Next.Date - voerx1.Date).Days;
                var avgVoerX1 = (float)(voerx1.Amount) / diffVoerx1Dates;

                // verschil in dagen tussen wx-1 en eerst volgende voer aankoop.
                var endDate = weights[i].Date < foodEmptyDate ? weights[i].Date : foodEmptyDate;
                var foodUseIndaysX1 = (endDate - dateWx1.Date).Days;

                // berken het gegeten voer in deze dagen.
                var foodUseX1 = foodUseIndaysX1 * avgVoerX1;

                // ----------------------

                // vind voer die het dichts bij datewx ligt en kleiner of gelijk aan is.
                var dateWx = weights[i].Date;
                var closestVoerDateWxIndex = GetClosestNextDateTimeIndex(foodPurchases, dateWx);

                // bereken hoeveel hiervan gem per dag gegeten word.
                //FoodPurchase voerx = null;
                FoodPurchase voerxPrev;
                int diffVoerxDates;
                float avgVoerX;
                float foodUseX;

                if (closestVoerDateWxIndex == -1 || (foodPurchases[closestVoerDateWxIndex].Date >= weights[i].Date && foodPurchases[closestVoerDateWxIndex - 1] == voerx1))
                {
                    foodUseX = 0;
                }
                else
                {
                    // bereken hoeveel hiervan gem per dag gegeten word.
                    voerxPrev = foodPurchases[closestVoerDateWxIndex - 1];
                    var foodEmptyDateX = foodPurchases[closestVoerDateWxIndex].Date;
                    diffVoerxDates = (foodEmptyDateX - voerxPrev.Date).Days;
                    avgVoerX = (float)(voerxPrev.Amount) / diffVoerxDates;

                    // verschil in dagen tussen wx en eerst volgende voer aankoop.
                    var foodUseIndaysX = (dateWx.Date - voerxPrev.Date).Days;

                    // berken het gegeten voer in deze dagen.
                    foodUseX = foodUseIndaysX * avgVoerX;                    
                }

                // ---------------------

                // optellen al het gegeten voedsel
                var foodSum = 0.0;
                var index = closestVoerDateWx1Index + 1;
                while (index < closestVoerDateWxIndex - 1)
                {
                    foodSum += foodPurchases[index].Amount;
                    index += 1;
                }

                foodSum += foodUseX1;
                foodSum += foodUseX;

                // voeder conversie berekenen.
                var vc = (float)foodSum / (weights[i].Weight - weights[i-1].Weight);

                result.Add(age, vc);
                }

            return result;
        }

        public async Task<PigGroup> Create(PigGroup pigGroup)
        {
            await _context.PigGroups.AddAsync(pigGroup);
            await _context.SaveChangesAsync();

            return pigGroup;
        }

        public async Task<PigGroup> Update(Guid id, PigGroup updatedItem)
        {
            updatedItem.Id = id;
            await Task.Run(() => _context.Entry(updatedItem).State = EntityState.Modified);
            await _context.SaveChangesAsync();
            return updatedItem;
        }
    }
}