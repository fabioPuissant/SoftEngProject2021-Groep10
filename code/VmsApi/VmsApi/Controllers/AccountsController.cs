using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VmsApi.Data.Models;
using VmsApi.Data.Utils;
using VmsApi.Services;
using VmsApi.ViewModels;
using VmsApi.ViewModels.PostModels;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using VmsApi.Exceptions;
using VmsApi.ViewModels.GetModels;

namespace VmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public AccountsController(IMapper mapper, UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Register", Name = "Register")]
        public async Task<ActionResult> Register([FromBody] UserRegistrationModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            user.UserName = userModel.Email;
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, AppRolesDict.ApplicationRoles["EMPLOYEE"].NormalizedName);
            if (AppRolesDict.ApplicationRoles.ContainsKey(userModel.Role))
            {
                await _userManager.AddToRoleAsync(user, userModel.Role);
            }
         
            // Status code 201 == Created!
            return StatusCode(201);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _userManager.Users.Include(u=>u.AssignedTasks).ThenInclude(at=> at.TaskItem).ToListAsync();
                IList<UserGetModel> models = new List<UserGetModel>(users.Count);

                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    models.Add(
                        new UserGetModel
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Id = u.Id,
                            AssignedTasks = u.AssignedTasks,
                            Username = u.UserName,
                            Roles = roles
                        });
                }
               
                return Ok(models);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return BadRequest();
            }
        }

        [HttpPost("Login", Name = "LoginEndpoint")]
        public async Task<IActionResult> LoginEndpoint(UserLoginModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var token = await _tokenGenerator.GetTokenAsync(user);

                return Ok(token);
            }

            return Unauthorized("Invalid Authentication");
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpDelete]
        [Route("Delete/{userid:guid}", Name = "DeleteUserById")]
        public async Task<IActionResult> DeleteUserEndpoint(Guid userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString());
                if (user == null)
                {
                    return NotFound(new ErrorMessageResult(message: $"No user found with {userid}", url: "Delete/{userid:guid}"));
                }

                await _userManager.DeleteAsync(user);
                return NoContent();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION IN DELETE USER-BY-ID] {exception.Source} {exception.Message}");
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("Roles/{userid:guid}")]
        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> AssignRolesToUser([FromRoute] Guid userid, [FromBody] ChangeRolesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("Model state of sent body is invalid"));
            }

            var found = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userid.ToString());
            if (found == null)
            {
                return NotFound(new ErrorMessageResult($"No User found with id of {userid}"));
            }

            try
            {
                ValidateRoles(model.Roles);
                model.UserId = userid;
                await UpdateUserRoles(found, model.Roles);
                await _userManager.UpdateAsync(user: found);
                return NoContent();
            }
            catch (RolesException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest(new ErrorMessageResult(ex.Message));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest(new ErrorMessageResult("Internal problem"));

            }
        }

        [HttpPut]
        [Authorize]
        [Route("Update/{userid:guid}")]
        public async Task<IActionResult> UpdateUser(Guid userid, [FromBody] UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("Model state of sent body is invalid"));
            }

            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userid.ToString());
                if (user == null)
                {
                    return NotFound(new ErrorMessageResult($"No User found with id of {userid}"));
                }

                await UpdateUserProps(user, model);
                var token = await _tokenGenerator.GetTokenAsync(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}] > {ex.Message}");
                return BadRequest(new ErrorMessageResult("Internal problem"));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("{userid:guid}/ChangePassword")]
        public async Task<IActionResult> UpdatePassword(Guid userid, PasswordResetModel model)
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"[INFO-{MethodBase.GetCurrentMethod()}] > Invalid modelstate ");

                return BadRequest(new ErrorMessageResult("Model state of sent body is invalid"));
            }

            if (model.Password != model.ConfirmPassword)
            {
                System.Diagnostics.Debug.WriteLine($"[INFO-{MethodBase.GetCurrentMethod()}] > Mismatch ");

                return BadRequest(new ErrorMessageResult("Newly chosen passwords do not match"));
            }

            var found = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userid.ToString());
            if (null == found)
            {
                return NotFound(new ErrorMessageResult($"No User found with id of {userid}"));
            }

            if (!await _userManager.CheckPasswordAsync(found, model.OldPassword))
            {
                return Unauthorized(new ErrorMessageResult("The old password is not correct"));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(found);
            var result = await _userManager.ResetPasswordAsync(found, token, model.ConfirmPassword);
            if (result.Succeeded)
            {
                System.Diagnostics.Debug.WriteLine($"[INFO-{MethodBase.GetCurrentMethod()}] > Password changed");
                return Ok();
            }
            return BadRequest(new ErrorMessageResult("Internal problem"));
        }

        [ExcludeFromCodeCoverage]
        private async Task UpdateUserProps(User user, UserUpdateModel model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
        }

        [ExcludeFromCodeCoverage]
        private void ValidateRoles(List<string> modelRoles)
        {
            foreach (var role in modelRoles)
            {
                if (!AppRolesDict.ApplicationRoles.ContainsKey(role))
                {
                    throw new RolesException($"{role} is not a valid user role");
                }
            }
        }

        [ExcludeFromCodeCoverage]
        private async Task UpdateUserRoles(User user, List<string> newRoles)
        {
            var rolesOfUser = await _userManager.GetRolesAsync(user);
            System.Diagnostics.Debug.WriteLine($"\n\n[FABIO-change roles {user.Email}] > {rolesOfUser.Join(",")}");
            System.Diagnostics.Debug.WriteLine($"[FABIO-new roles {user.Email}] :: {newRoles.Join("---")}\n\n");

            foreach (var oldRole in rolesOfUser)
            {
                string name = AppRolesDict.ApplicationRoles.Values.Where(n => n.Name == oldRole)
                    .Select(m => m.NormalizedName).First();
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                await _userManager.RemoveFromRoleAsync(user, name);
            }

            rolesOfUser = await _userManager.GetRolesAsync(user);
            System.Diagnostics.Debug.WriteLine($"### [FABIO-current roles {user.Email}] :: {rolesOfUser.Join("---")} ### \n");

            foreach (var role in newRoles)
            {
               
                await _userManager.AddToRoleAsync(user, role);
                var name = 
                await _userManager.AddToRoleAsync(user, role);
            }

            rolesOfUser = await _userManager.GetRolesAsync(user);
            System.Diagnostics.Debug.WriteLine($"\n\n[FABIO-change roles {user.Email}] > {rolesOfUser.Join(" <> ")}\n");
        }
    }
}