using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.ViewModels.PostModels
{
    [ExcludeFromCodeCoverage]
    public class AssignTaskToUserModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid TaskId { get; set; }
    }
}
