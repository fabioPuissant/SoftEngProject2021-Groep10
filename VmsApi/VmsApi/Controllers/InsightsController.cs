using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VmsApi.Data.Repositories;
using VmsApi.Data.Repositories.interfaces;

namespace VmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class InsightsController : ControllerBase
    {
        private readonly IPigGroupRepo _pigGroupRepo;

        public InsightsController(IPigGroupRepo pigGroupRepo)
        {
            _pigGroupRepo = pigGroupRepo;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("VCGroups/{id}")]
        public async Task<ActionResult> GetVCGroups(Guid id)
        {
            return Ok( await _pigGroupRepo.GetVCGroup(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("GrowthGroups/{id}")]
        public async Task<ActionResult> GetGrowthGroups(Guid id)
        {
            return Ok(await _pigGroupRepo.GetGrowthGroup(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("FoodUseGroups/{id}")]
        public async Task<ActionResult> GetFoodUseGroups(Guid id)
        {
            return Ok(await _pigGroupRepo.GetFoodUseGroup(id));
        }
    }
}
