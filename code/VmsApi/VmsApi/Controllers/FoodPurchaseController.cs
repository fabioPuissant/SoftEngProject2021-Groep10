using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VmsApi.Data.Exceptions;
using VmsApi.Data.Models;
using VmsApi.Data.Repositories;
using VmsApi.Mappers;
using VmsApi.ViewModels;

namespace VmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodPurchaseController : ControllerBase
    {
        private readonly IFoodPurchasesRepo _foodPurchaseRepo;
        private readonly IMappings _mappings;

        public FoodPurchaseController(IFoodPurchasesRepo foodPurchasesRepo, IMappings mappings)
        {
            _foodPurchaseRepo = foodPurchasesRepo;
            _mappings = mappings;
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder")]
        [HttpGet]
        [Route("Purchases")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var purchases = await _foodPurchaseRepo.GetAll();
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder")]
        [HttpGet]
        [Route("Purchases/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var purchase = await _foodPurchaseRepo.GetByIdAsync(id);
                if (purchase == null)
                {
                    return NotFound(new ErrorMessageResult($"No FoodPurchase Found with Id {id}"));
                }

                return Ok(purchase);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder")]
        [HttpPost]
        [Route("Purchases")]
        public async Task<IActionResult> CreateAsync(PostMapperFoodPurchase newMapperPurchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("Purchase model is invalid"));
            }
            try
            {
                var newPurchase = _mappings.MapToFoodPurchase(newMapperPurchase);
                var createFoodPurchase = await _foodPurchaseRepo.CreateAsync(newMapperPurchase.GroupNumber, newPurchase);

                return Ok(createFoodPurchase);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder")]
        [HttpPut]
        [Route("Purchases/{id}")]
        public async Task<IActionResult> Update(Guid id, FoodPurchase newPurchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorMessageResult("Invalid FoodPurchase model"));
            }

            try
            {
                Console.WriteLine("debug info API CALL: create");
                var updatedFoodPurchase = await _foodPurchaseRepo.UpdateAsync(id, newPurchase);

                return Ok(updatedFoodPurchase);
            }
            catch (IdException e)
            {
                Console.WriteLine();
                return NotFound(new ErrorMessageResult($"No FoodPurchaseFound with id {id}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator,Manager,Voedingsbeheerder")]
        [HttpDelete]
        [Route("Purchases/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var foodPurchase = await _foodPurchaseRepo.GetByIdAsync(id);
                if (foodPurchase == null)
                {
                    return NotFound(new ErrorMessageResult(message: $"No food entry found with {id}", url: "Purchases/{id}"));
                }

                await _foodPurchaseRepo.DeleteAsync(foodPurchase);
                return NoContent();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPTION IN DELETE FOOD-ENTRY-BY-ID] {exception.Source} {exception.Message}");
                return BadRequest();
            }
        }
    }
}