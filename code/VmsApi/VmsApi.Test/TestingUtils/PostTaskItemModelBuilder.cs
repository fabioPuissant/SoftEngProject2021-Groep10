using System;
using System.Diagnostics.CodeAnalysis;
using VmsApi.Data.Models;
using VmsApi.ViewModels.PostModels;

namespace VmsApi.Test
{
    [ExcludeFromCodeCoverage]
    public class PostTaskItemModelBuilder
    {
        private PostTaskItemModel _item;

        public PostTaskItemModelBuilder()
        {
            _item = new PostTaskItemModel();
        }

        public PostTaskItemModelBuilder WithDueDate(DateTime dueDate)
        {
            _item.DueDate = dueDate;
            return this;
        }

        public PostTaskItemModelBuilder WithStartDate(DateTime startDate)
        {
            _item.StartDate= startDate;
            return this;
        }

        public PostTaskItemModelBuilder WithTaskTitle(string title)
        {
            _item.TaskTitle = title;
            return this;
        }

        public PostTaskItemModelBuilder WithDescription(string description)
        {
            _item.Description = description;
            return this;
        }

        public PostTaskItemModelBuilder WithRepeatingIntervalDays(int repeatingIntervalDays)
        {
            _item.RepeatingIntervalDays = repeatingIntervalDays;
            return this;
        }

        public PostTaskItemModelBuilder WithCompleted(bool completed)
        {
            _item.Completed = completed;
            return this;
        }

        public PostTaskItemModel Build()
        {
            return _item;
        }
    }
}
