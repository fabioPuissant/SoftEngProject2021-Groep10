using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.Mappers;
using VmsApi.ViewModels;

namespace VmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurePointController : ControllerBase
    {
        private readonly IMeasurePointRepo _measurePointRepo;
        private readonly IMappings _mappings;
        public MeasurePointController(IMeasurePointRepo measurePointReo, IMappings mappings)
        {
            _measurePointRepo = measurePointReo;
            _mappings = mappings;
        }

        [Authorize(Roles = "Administrator,Manager,Weger")]
        [HttpGet]
        [Route("Points")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var points = await _measurePointRepo.GetAll();
                return Ok(points);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest();
            }
           
        }

        [Authorize(Roles = "Administrator,Manager,Weger")]
        [HttpGet]
        [Route("Points/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var point = await _measurePointRepo.GetById(id);
                if (point == null)
                {
                    return NotFound(new ErrorMessageResult($"No MeasurePoint found with Id of {id}"));
                }

                return Ok(point);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager,Weger")]
        [HttpPost]
        [Route("Points")]
        public async Task<IActionResult> Create(PostMapperMeasurePoint newMapperPoint)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessageResult("PostMapperMeasurePoint model is invalid"));
                }

                Console.WriteLine("debug info API CALL: create");
                var newPoint = _mappings.MapToMeasurePoint(newMapperPoint);
                var createdMeasurePoint = await _measurePointRepo.Create(newMapperPoint.GroupNumber, newPoint);

                return Ok(createdMeasurePoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager,Weger")]
        [HttpPut]
        [Route("Points/{id}")]
        public async Task<IActionResult> Update(Guid id, MeasurePoint updatedMeasurePoint)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessageResult("Invalid MeasurePoint model"));
                }

                Console.WriteLine("debug info API CALL: create");
                var updatedResult = await _measurePointRepo.Update(id, updatedMeasurePoint);
                return Ok(updatedResult);
            }
            catch (IdException e)
            {
                return NotFound(new ErrorMessageResult(e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest();
            }

        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder")]
        [HttpDelete]
        [Route("Points/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var foodPurchase = await _measurePointRepo.GetById(id);
                if (foodPurchase == null)
                {
                    return NotFound(new ErrorMessageResult(message: $"No measurepoint found with {id}", url: "Points/{id}"));
                }

                await _measurePointRepo.DeleteAsync(foodPurchase);
                return NoContent();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION IN DELETE MEASUREPOINT-BY-ID] {exception.Source} {exception.Message}");
                return BadRequest();
            }
        }

    }
}
