using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VmsApi.Data.Mappers;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories.interfaces
{
    public interface IPigGroupRepo
    {
        Task<List<DetailedPigGroup>> GetAll();
        Task<PigGroup> GetById(Guid id);
        Task<List<DetailedPigGroup>> GetAllByDate(DateTime? startDateTime, DateTime? endDateTime);
        Task<PigGroup> Create(PigGroup pigGroup);
        Task<PigGroup> Update(Guid id, PigGroup updatedItem);
        Task<Dictionary<int, float>> GetVCGroup(Guid id);
        Task<Dictionary<int, float>> GetGrowthGroup(Guid id);
        Task<Dictionary<int, float>> GetFoodUseGroup(Guid id);

    }
}