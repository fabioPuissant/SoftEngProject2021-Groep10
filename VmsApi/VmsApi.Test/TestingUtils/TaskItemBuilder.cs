using System;
using System.Diagnostics.CodeAnalysis;
using VmsApi.Data.Models;

namespace VmsApi.Test
{
    [ExcludeFromCodeCoverage]
    public class TaskItemBuilder
    {
        private TaskItem _item;

        public TaskItemBuilder()
        {
            _item = new TaskItem();
        }

        public TaskItemBuilder WithId(Guid id)
        {
            _item.Id = id;
            return this;
        }

        public TaskItemBuilder WithDueDate(DateTime dueDate)
        {
            _item.DueDate = dueDate;
            return this;
        }

        public TaskItemBuilder WithStartDate(DateTime startDate)
        {
            _item.StartDate= startDate;
            return this;
        }

        public TaskItemBuilder WithTaskTitle(string title)
        {
            _item.TaskTitle = title;
            return this;
        }

        public TaskItemBuilder WithDescription(string description)
        {
            _item.Description = description;
            return this;
        }

        public TaskItemBuilder WithRepeatingIntervalDays(int repeatingIntervalDays)
        {
            _item.RepeatingIntervalDays = repeatingIntervalDays;
            return this;
        }

        public TaskItemBuilder WithCompleted(bool completed)
        {
            _item.Completed = completed;
            return this;
        }

        public TaskItem Build()
        {
            return _item;
        }
    }
}
