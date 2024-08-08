using HotDeskBookingSystem.Data.Dto.Booking;
using HotDeskBookingSystem.Data.Dto.User;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Interfaces.Services;
using HotDeskBookingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotDeskBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public BookingController(IBookingRepository bookingRepository, IJwtTokenService jwtTokenService)
        {
            _bookingRepository = bookingRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{bookingId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpGet("status/{bookingStatusName}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllBookingsByBookingStatusAsync(string bookingStatusName)
        {
            var bookings = await _bookingRepository.GetAllBookingsByBookingStatusAsync(bookingStatusName);
            return Ok(bookings);
        }

        [HttpGet("email/{email}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetBookingsByUserEmailAsync(string email)
        {
            var requestingUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var requestingUserRoles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
            if (requestingUserEmail != email && !requestingUserRoles.Contains("ADMIN"))
            {
                return Unauthorized("You do not have permission to update this user");
            }

            var bookings = await _bookingRepository.GetBookingsByUserEmailAsync(email);
            return Ok(bookings);
        }

        [HttpGet("desk/{deskId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetBookingsByDeskIdAsync(int deskId)
        {
            var bookings = await _bookingRepository.GetBookingsByDeskIdAsync(deskId);
            return Ok(bookings);
        }

        [HttpPost]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> BookDeskAsync([FromBody] CreateBookingDto bookingDto)
        {
            var booking = await _bookingRepository.BookDeskAsync(bookingDto);
            if (booking == null)
            {
                return BadRequest("Booking failed");
            }
            return Ok(booking);
        }

        [HttpPut("{bookingId}")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> ChangeReservationAsync(int bookingId, [FromBody] Booking newBooking)
        {
            var booking = await _bookingRepository.ChangeReservationAsync(bookingId, newBooking);
            if (booking == null)
            {
                return BadRequest("Reservation change failed");
            }
            return Ok(booking);
        }

        [HttpDelete("bookingId")]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> DeleteBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return NotFound($"Booking with id {bookingId} not found");
            }
            var requestingUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var requestingUserRoles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
            if (booking.UserEmail != requestingUserEmail && !requestingUserRoles.Contains("ADMIN"))
            {
                return Unauthorized("You do not have permission to delete this booking");
            }

            var result = await _bookingRepository.DeleteBookingManuallyAsync(booking);
            if (result != true)
            {
                return BadRequest("Booking deletion failed");
            }
            return Ok(booking);
        }


    }
}
