using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.ViewModels.PostModels
{
    [ExcludeFromCodeCoverage]
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
