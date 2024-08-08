using HotDeskBookingSystem.Data.Dto.Booking;
using HotDeskBookingSystem.Data.Dto.Desk;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeskController : ControllerBase
    {
        private readonly IDeskRepository _deskRepository;

        public DeskController(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
        }

        [HttpGet("{deskId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetDeskByIdAsync(int deskId)
        {
            var desk = await _deskRepository.GetDeskByIdAsync(deskId);
            if (desk == null)
            {
                return NotFound();
            }
            return Ok(desk);
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllDesksAsync()
        {
            var desks = await _deskRepository.GetAllDesksAsync();
            return Ok(desks);
        }

        [HttpGet("floor/{officeFloorId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetDesksByOfficeFloorIdAsync(int officeFloorId)
        {
            var desks = await _deskRepository.GetDesksByOfficeFloorIdAsync(officeFloorId);
            return Ok(desks);
        }

        [HttpGet("office/{officeId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetDesksByOfficeIdAsync(int officeId)
        {
            var desks = await _deskRepository.GetDesksByOfficeIdAsync(officeId);
            return Ok(desks);
        }

        [HttpPost("office/available/{officeId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetAvailableDesksByOfficeIdAsync(int officeId, [FromBody] ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.End <= reservationTimes.Start && reservationTimes.Start >= DateTime.Now.AddDays(1))
            {
                return BadRequest("Invalid reservation times");
            }

            var desks = await _deskRepository.GetAvailableDesksByOfficeIdAsync(officeId, reservationTimes);
            return Ok(desks);
        }


        [HttpPost("floor/available/{floorId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetAvailableDesksByOfficeFloorIdAsync(int floorId, [FromBody] ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.End <= reservationTimes.Start && reservationTimes.Start >= DateTime.Now.AddDays(1))
            {
                return BadRequest("Invalid reservation times");
            }

            var desks = await _deskRepository.GetAvailableDesksByOfficeFloorIdAsync(floorId, reservationTimes);
            return Ok(desks);
        }


        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddDeskAsync([FromBody] DeskCreationDto deskDto)
        {
            var desk = new Desk
            {
                OfficeFloorId = deskDto.OfficeFloorId
            };
            var addedDesk = await _deskRepository.AddDeskAsync(desk);
            if (addedDesk == null)
            {
                return StatusCode(500, "Error creating new desk");
            }
            return Ok(addedDesk);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateDeskAsync([FromBody] Desk desk)
        {
            var updatedDesk = await _deskRepository.UpdateDeskAsync(desk);
            if (updatedDesk == null)
            {
                return StatusCode(500, $"Error updating desk {desk.DeskId} on floor" +
                    $" {desk.OfficeFloorId} with number {desk.OfficeFloor.FloorNumber}" +
                    $" in the {desk.OfficeFloor.OfficeId} office");
            }
            return Ok(updatedDesk);
        }

        [HttpDelete("{deskId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteDeskAsync(int deskId)
        {
            var deletedDesk = await _deskRepository.DeleteDeskAsync(deskId);
            if (deletedDesk == null)
            {
                return StatusCode(500, $"Error deleting desk {deskId}");
            }
            return Ok(deletedDesk);
        }
    }
}
