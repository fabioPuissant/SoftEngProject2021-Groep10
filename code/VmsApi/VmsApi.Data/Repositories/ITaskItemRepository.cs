using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories
{
    public interface ITaskItemRepository
    {
        Task<TaskItem> GetByIdAsync(Guid id);
        Task<IList<TaskItem>> GetAllAsync();
        Task AddAsync(TaskItem item);
        Task UpdateAsync(Guid id, TaskItem updatedItem);
        Task DeleteAsync(TaskItem item);
        Task<bool> IsPresentAsync(Guid id);
        Task AddUserToTask(Guid taskId, Guid userId);
        Task SaveAsync();
    }
}
