using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.Data.Repositories.interfaces;
using VmsApi.ViewModels;

namespace VmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PigGroupController : ControllerBase
    {
        private readonly IPigGroupRepo _pigGroupRepo;

        public PigGroupController(IPigGroupRepo pigGroupRepo)
        {
            _pigGroupRepo = pigGroupRepo;
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder,Weger")]
        [HttpGet]
        [Route("groups")]
        public async Task<IActionResult> GetAll()
        {
            Console.WriteLine("debug info API CALL: group get all call");
            var groups = await _pigGroupRepo.GetAll();
            return Ok(groups);
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder,Weger")]
        [HttpGet]
        [Route("groups/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Console.WriteLine("debug info API CALL: group id call");
            var group = await _pigGroupRepo.GetById(id);
            return Ok(group);
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder,Weger")]
        [HttpGet]
        [Route("groups/birthdate")]
        public async Task<IActionResult> GetByDate(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                Console.WriteLine("debug info API CALL: group by date");
                Console.WriteLine($"URL::groups/birthdate>START-DATE::{startDate}");
                Console.WriteLine($"URL::groups/birthdate>END-DATE::{endDate}");
                var groups = await _pigGroupRepo.GetAllByDate(startDate, endDate);

                return Ok(groups);
            }
            catch (DatePigGroupException ex)
            {
                Console.WriteLine($"URL::groups/birthdate>DatePigGroupException::{ex.Message}");
                return BadRequest(new ErrorMessageResult(ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"URL::groups/birthdate>Exception::{ex.StackTrace}");
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager")]
        [HttpPost]
        [Route("groups")]
        public async Task<IActionResult> Create(PigGroup pigGroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessageResult("PigGroup model state is invalid"));
                }

                Console.WriteLine("debug info API CALL: create");
                var createdGroup = await _pigGroupRepo.Create(pigGroup);

                return Ok(createdGroup);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageResult(e.Message, "groups"));
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut]
        [Route("groups/{id}")]
        public async Task<IActionResult> Update(Guid id, PigGroup existingGroup)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessageResult("PigGroup model state is invalid"));
                }

                await _pigGroupRepo.Update(id, existingGroup);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorMessageResult(e.Message, "groups"));
            }
        }
    }
}