using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.ViewModels.PostModels
{
    [ExcludeFromCodeCoverage]
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
