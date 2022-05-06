using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VmsApi.Data.DataDbContext;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;

namespace VmsApi.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TaskItemRepository : ITaskItemRepository
    {

        private readonly VmsDbContext _context;

        public TaskItemRepository(VmsDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> GetByIdAsync(Guid id)
        {
            var found = await _context.TaskItems
                .Include(x => x.AssignedTasks)
                .ThenInclude(y => y.User)
                .FirstOrDefaultAsync(x => x.Id.ToString() == id.ToString());
            if (found == null) throw new NoEntityFoundException($"No TaskItem found with Id of {id}");
            return found;
        }

        public async Task<IList<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task AddAsync(TaskItem item)
        {
            if (item.Id != Guid.Empty) throw new IdException("Id of new item was set before adding to database!");
            await _context.AddAsync(item);
        }

        public async Task UpdateAsync(Guid id, TaskItem updatedItem)
        {
            if (id == Guid.Empty) throw new IdException("Given id for to be updated TaskItem was instance of empty Guid");
            updatedItem.Id = id;
            await Task.Run(() => _context.Entry(updatedItem).State = EntityState.Modified);
        }

        public async Task DeleteAsync(TaskItem item)
        {
            await Task.Run(() => _context.TaskItems.Remove(item));
        }

        public async Task<bool> IsPresentAsync(Guid id)
        {
            return await GetByIdAsync(id) != null;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToTask(Guid taskId, Guid userId)
        {
            var taskItem = await GetByIdAsync(taskId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId.ToString());
            if (user == null) { throw new NoEntityFoundException($"No User found with Id of {userId.ToString()}"); }
            
            bool alreadyAssigned = IsUserAlreadyAssigned(taskItem, user.Id);
            if (alreadyAssigned)
            {
                throw new TaskUserAssignedException($"User: {user.Id} ({user.FirstName} {user.LastName}) was already assigned to task: {taskItem.TaskTitle}-{taskItem.Id}");
            }
            
            AssignedTask newAssignedTask = new AssignedTask
            {
                User = user,
                UserId = user.Id,
                TaskItem = taskItem,
                TaskItemId = taskItem.Id
            };
            taskItem.AssignedTasks.Add(newAssignedTask);
        }

        private bool IsUserAlreadyAssigned(TaskItem taskItem, string userId)
        {
            var result = taskItem.AssignedTasks.ToList().FirstOrDefault(x => x.UserId == userId);
            return result != null;
        }
    }


}
