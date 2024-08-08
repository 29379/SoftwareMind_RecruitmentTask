using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskBookingSystem.Controllers
{
    [Route("api/floor")]
    [ApiController]
    public class OfficeFloorController : ControllerBase
    {
        private readonly IOfficeFloorRepository _officeFloorRepository;

        public OfficeFloorController(IOfficeFloorRepository officeFloorRepository)
        {
            _officeFloorRepository = officeFloorRepository;
        }

        [HttpGet("{floorId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetOfficeFloorByIdAsync(int floorId)
        {
            var floor = await _officeFloorRepository
                .GetOfficeFloorByIdAsync(floorId);
            if (floor == null)
            {
                return NotFound($"Office floor with id {floorId} was not found.");
            }
            return Ok(floor);
        }

        [HttpGet("office/{officeId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetAllOfficeFloorsByOfficeIdAsync(int officeId)
        {
            var floors = await _officeFloorRepository
                .GetAllOfficeFloorsByOfficeIdAsync(officeId);
            if (floors == null)
            {
                return NotFound("Something went wrong. No floors were found for office id {officeId}.");
            }
            return Ok(floors);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddOfficeFloorAsync([FromBody] OfficeFloorDto newFloor)
        {
            var addedFloor = await _officeFloorRepository
                .AddOfficeFloorAsync(newFloor);
            if (addedFloor == null)
            {
                return StatusCode(500, "Error occured during creating a new office floor");
            }
            return Ok(addedFloor);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateOfficeFloorAsync([FromBody] OfficeFloor changedFloor)
        {
            var updatedFloor = await _officeFloorRepository.UpdateOfficeFloorAsync(changedFloor);
            if (updatedFloor == null)
            {
                return StatusCode(500, $"Error occured while updating office floor with id {changedFloor.OfficeFloorId}");
            }
            return Ok(updatedFloor);
        }

        [HttpDelete("{floorId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteOfficeFloorAsync(int floorId)
        {
            var deletedFloor = await _officeFloorRepository.DeleteOfficeFloorAsync(floorId);
            if (deletedFloor == null)
            {
                return StatusCode(500, $"Error occured whil deleting office floor: {floorId}");
            }
            return Ok(deletedFloor);
        }
    }
}
