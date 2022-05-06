using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.ViewModels;
using VmsApi.ViewModels.PostModels;


namespace VmsApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemRepository _repository;
        private readonly IMapper _mapper;

        public TaskItemsController(ITaskItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/<TaskItemsController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var resultsAsync = await _repository.GetAllAsync();
            return Ok(resultsAsync);
        }

        // GET api/<TaskItemsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var taskItem = await _repository.GetByIdAsync(id);
                return Ok(taskItem);
            }
            catch (IdException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return NotFound(new ErrorMessageResult($"No TaskITem Found with Id {id}"));
            }
        }

        // POST api/<TaskItemsController> 
        [HttpPost]
        [Authorize(Roles = "Administrator,Manager,Agenda")]
        public async Task<IActionResult> PostAsync([FromBody] PostTaskItemModel taskItemModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("TaskItem model is invalid"));
            }

            try
            {
                TaskItem item = _mapper.Map<TaskItem>(taskItemModel);
                await _repository.AddAsync(item);
                await _repository.SaveAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest();
            }

        }

        // PUT api/<TaskItemsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] PostTaskItemModel taskItem)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("TaskItem model is invalid"));
            }

            try
            {
                var item = _mapper.Map<TaskItem>(taskItem);
                await _repository.UpdateAsync(id, item);
                await _repository.SaveAsync();
                return Ok(item);
            }
            catch (IdException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return NotFound(new ErrorMessageResult($"No TaskITem Found with Id {id}"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{taskid:guid}")]
        public async Task<IActionResult> AssignUserToTaskPostAsync(Guid taskid, [FromBody] AssignTaskToUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("Model is invalid"));
            }

            try
            {
                if (taskid != model.TaskId)
                {
                    return BadRequest(new ErrorMessageResult(
                        $"Task ID's do not match. Id in Route was: {taskid.ToString()}, Id in Body was {model.TaskId.ToString()}"));
                }

                await _repository.AddUserToTask(taskid, model.UserId);
                await _repository.SaveAsync();
                return NoContent();
            }
            catch (NoEntityFoundException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return NotFound(new ErrorMessageResult(ex.Message));
            }
            catch (TaskUserAssignedException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest(new ErrorMessageResult(ex.Message));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest();
            }
        }

        // DELETE api/<TaskItemsController>/5
        [Authorize(Roles = "Administrator,Manager,Agenda")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var found = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(found);
                await _repository.SaveAsync();
                return NoContent();
            }
            catch (IdException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return NotFound(new ErrorMessageResult($"No TaskITem Found with Id {id}"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR-{MethodBase.GetCurrentMethod()}]> {ex.Message}");
                return BadRequest();
            }
        }
    }
}
