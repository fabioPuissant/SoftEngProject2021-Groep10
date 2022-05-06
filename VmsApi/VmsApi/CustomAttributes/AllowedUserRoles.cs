using System.ComponentModel.DataAnnotations;
using System.Linq;
using VmsApi.ViewModels.PostModels;

namespace VmsApi.CustomAttributes
{
    public class AllowedUserRoles : ValidationAttribute
    {
        private string[] _allowedValues;

        public AllowedUserRoles(string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = value as ChangeRolesModel;
            if (model == null)
            {
                return new ValidationResult($"Failed to parse {value} to an instance of UserUpdateModel");
            }

            
            if (model?.Roles == null || model.Roles.Count == 0)
            {
                return new ValidationResult("No Roles where given!");
            }
            foreach (string role in model.Roles)
            {
                if (!_allowedValues.Contains(role))
                {
                    return new ValidationResult($"{ role } is not a valid status");
                }
            }
            
            return ValidationResult.Success;
        }
    }
}