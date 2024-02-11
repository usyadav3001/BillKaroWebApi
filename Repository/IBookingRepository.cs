using StudioWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public interface IBookingRepository
    {
        public Result GetAllBookings(Booking booking);

        public Result GetBookingById(Booking booking);

        public Result AddBooking(Booking booking);
        public string AddBookingDetail(List<BookingDetail> bookingDetail);

        public Result UpdateBooking(Booking booking);

        public Result DeleteBooking(Booking booking);

    }
}
