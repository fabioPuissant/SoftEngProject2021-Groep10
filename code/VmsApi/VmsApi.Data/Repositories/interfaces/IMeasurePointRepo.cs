using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories
{
    public interface IMeasurePointRepo
    {
        Task<List<MeasurePoint>> GetAll();
        Task<MeasurePoint> GetById(Guid id);
        Task<MeasurePoint> Create(string groupNumber, MeasurePoint measurePoint);
        Task<MeasurePoint> Update(Guid id, MeasurePoint updatedItem);
        Task DeleteAsync(MeasurePoint item);
    }
}