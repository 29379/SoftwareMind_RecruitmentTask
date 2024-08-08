using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeRepository _officeRepository;

        public OfficeController(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        [HttpGet("{officeId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetOfficeByIdAsync(int officeId)
        {
            var office = await _officeRepository.GetOfficeByIdAsync(officeId);
            if (office == null)
            {
                return NotFound($"Office with id {officeId} was not found.");
            }
            return Ok(office);
        }

        [HttpGet]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetAllOfficesAsync()
        {
            var offices = await _officeRepository.GetAllOfficesAsync();
            return Ok(offices);
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddOfficeAsync([FromBody] OfficeDto office)
        {
            var addedOffice = await _officeRepository
                .AddOfficeAsync(office);
            if (addedOffice == null)
            {
                return StatusCode(500, $"Error occured while creating a new office: {office.ToString()}");
            }
            return Ok(addedOffice);
        }

        [HttpPut]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateOfficeAsync([FromBody] Office office)
        {
            var updatedOffice = await _officeRepository.UpdateOfficeAsync(office);
            if (updatedOffice == null)
            {
                return StatusCode(500, $"Error occured while updating office floor with id {office.OfficeId}");
            }
            return Ok(updatedOffice);
        }

        [HttpDelete("{officeId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteOfficeAsync(int officeId)
        {
            var deletedOffice = await _officeRepository.DeleteOfficeAsync(officeId);
            if (deletedOffice == null)
            {
                return StatusCode(500, $"Error occured whil deleting office floor: {officeId}");
            }
            return Ok(deletedOffice);
        }
    }
}
