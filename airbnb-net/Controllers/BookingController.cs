using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDataAccess.Models;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }
        
        // GET: api/Booking
        [HttpGet]
        public async Task<List<BookingModel>> Get()
        {
            return await _bookingService.GetAllBookings();
        }
        
        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<BookingModel> Get(string id)
        {
            return await _bookingService.GetBooking(id);
        }

        // POST: api/Booking
        [HttpPost]
        public async void Post([FromBody] BookingModel model)
        {
            await _bookingService.CreateBookingAsync(model);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] BookingModel model)
        {
            await _bookingService.UpdateBookingAsync(id, model);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _bookingService.DeleteBookingAsync(id);
        }
    }
}
