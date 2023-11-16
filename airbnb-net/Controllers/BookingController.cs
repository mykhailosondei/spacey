using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Booking;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : InternalControllerBase
    {
        private readonly IBookingQueryRepository _bookingQueryRepository;
        private readonly IBookingCommandAccess _bookingCommandAccess;
        private readonly IUserIdGetter _userIdGetter;

        public BookingController(IBookingQueryRepository bookingQueryRepository,
            IBookingCommandAccess bookingCommandAccess, ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter) : base(logger,mapper)
        {
            _bookingQueryRepository = bookingQueryRepository;
            _bookingCommandAccess = bookingCommandAccess;
            _userIdGetter = userIdGetter;
        }
        
        // GET: api/Booking
        [HttpGet]
        public async Task<IEnumerable<BookingDTO>> Get()
        {
            return _mapper.Map<IEnumerable<BookingDTO>>(await _bookingQueryRepository.GetAllBookings());
        }
        
        // GET: api/Booking/5
        [HttpGet("{id:guid}")]
        public async Task<BookingDTO> Get(Guid id)
        {
            return _mapper.Map<BookingDTO>(await _bookingQueryRepository.GetBookingById(id));
        }
        
        // POST: api/Booking
        [HttpPost]
        [Authorize]
        public async Task<Guid> Post([FromBody] BookingCreateDTO bookingCreate)
        {
            var bookingDTO = _mapper.Map<BookingDTO>(bookingCreate);
            var booking = _mapper.Map<Booking>(bookingDTO);
            booking.UserId = _userIdGetter.UserId;
            return await _bookingCommandAccess.AddBooking(booking);
        }
        
        // PUT: api/Booking/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] BookingUpdateDTO bookingUpdate)
        {
            var bookingDTO = _mapper.Map<BookingDTO>(bookingUpdate);
            var booking = _mapper.Map<Booking>(bookingDTO);
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
