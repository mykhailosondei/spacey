using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.Enums;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Helpers;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using CustomMediator;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class UpdateBookingCommandHandler : BaseHandler, IRequestHandler<UpdateBookingCommand>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    
    public UpdateBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingDTO = _mapper.Map<BookingDTO>(request.Booking);
        var booking = _mapper.Map<Booking>(bookingDTO);
        
        var existingBooking = await _bookingQueryRepository.GetBookingById(request.Id);
        
        if (existingBooking == null)
        {
            throw new NotFoundException("Booking");
        }
        
        booking.Id = existingBooking.Id;
        booking.ListingId = existingBooking.ListingId;
        
        if(existingBooking.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this booking.");
        }
        
        if(existingBooking.Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException("You cannot update a canceled booking.");
        }

        if (booking.CheckIn != existingBooking.CheckIn || booking.CheckOut != existingBooking.CheckOut)
        {
            var listingEntity = await _listingQueryRepository.GetListingById(booking.ListingId);
            
            if (listingEntity == null)
            {
                throw new NotFoundException("Listing");
            }

            var bookings = await _bookingQueryRepository.GetBookingsByListingId(booking.ListingId);
            
            var existingBookings = bookings.Where(b => b.Status == BookingStatus.Active && b.Id != booking.Id).Select(b => (b.CheckIn, b.CheckOut)).ToArray();
            
            if (BookingHelper.DateIntersects(booking.CheckIn, booking.CheckOut, existingBookings))
            {
                throw new InvalidOperationException("Listing is already booked for this period or a part of it.");
            }
        }
        
        await _bookingCommandAccess.UpdateBooking(request.Id ,booking);
    }
}