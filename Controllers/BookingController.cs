using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudioWebApi.Models;
using StudioWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository bookingRepository;
        public BookingController(IBookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }

        [Route("GetAllBookings")]
        [HttpPost]
        public IActionResult GetAllBookings(Booking booking)
        {
            var bookings = bookingRepository.GetAllBookings(booking);
            return Ok(bookings);
        }

        [Route("GetBookingById")]
        [HttpPost]
        public IActionResult GetBookingById(Booking booking)
        {
            var bookingData = bookingRepository.GetBookingById(booking);

            if (bookingData == null)
                return NotFound();

            return Ok(bookingData);
        }

        [Route("AddBooking")]
        [HttpPost]
        public IActionResult AddBooking([FromBody] Booking booking)
        {
            var result=bookingRepository.AddBooking(booking);
            return Ok(result);
        }

        [Route("UpdateBooking")]
        [HttpPost]
        public IActionResult UpdateBooking([FromBody] Booking booking)
        {
            var existingBooking = bookingRepository.GetBookingById(booking);

            if (existingBooking == null)
                return NotFound();

            var result = bookingRepository.UpdateBooking(booking);

            return Ok(result);
        }

        [Route("DeleteBooking")]
        [HttpPost]
        public IActionResult DeleteBooking(Booking booking)
        {
            var existingBooking = bookingRepository.GetBookingById(booking);

            if (existingBooking == null)
                return NotFound();

            var result = bookingRepository.DeleteBooking(booking);

            return Ok(result);
        }
    }
}
