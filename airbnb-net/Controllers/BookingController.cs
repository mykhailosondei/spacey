using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingQueryRepository _bookingQueryRepository;
        private readonly BookingCommandAccess _bookingCommandAccess;
        
        public BookingController(BookingQueryRepository bookingQueryRepository, BookingCommandAccess bookingCommandAccess)
        {
            _bookingQueryRepository = bookingQueryRepository;
            _bookingCommandAccess = bookingCommandAccess;
        }
        
        // GET: api/Booking
        [HttpGet]
        public async Task<IEnumerable<Booking>> Get()
        {
            return await _bookingQueryRepository.GetAllBookings();
        }
        
        // GET: api/Booking/5
        [HttpGet("{id:guid}")]
        public async Task<Booking> Get(Guid id)
        {
            return await _bookingQueryRepository.GetBookingById(id);
        }
        
        // POST: api/Booking
        [HttpPost]
        public async Task<Guid> Post([FromBody] Booking booking)
        {
            return await _bookingCommandAccess.AddBooking(booking);
        }
        
        // PUT: api/Booking/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] Booking booking)
        {
            await _bookingCommandAccess.UpdateBooking(id, booking);
        }
        
        // DELETE: api/Booking/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _bookingCommandAccess.DeleteBooking(id);
        }
    }
}
