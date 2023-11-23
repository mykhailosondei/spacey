using ApplicationCommon.DTOs.Booking;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Helpers;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class CreateBookingCommandHandler : BaseHandler, IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserIdGetter _userIdGetter;

    public CreateBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess, IListingQueryRepository listingQueryRepository, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter, IUserQueryRepository userQueryRepository) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
        _listingQueryRepository = listingQueryRepository;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
        _userQueryRepository = userQueryRepository;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingDTO = _mapper.Map<BookingDTO>(request.Booking);
        var booking = _mapper.Map<Booking>(bookingDTO);
        
        var listingEntity = await _listingQueryRepository.GetListingById(booking.ListingId);
        
        if (listingEntity == null)
        {
            throw new NotFoundException("Listing");
        }

        var existingBookings = listingEntity.BookingsIds.Select(bookingId =>
        {
            var bookingEntity = _bookingQueryRepository.GetBookingById(bookingId).Result;
            return (bookingEntity.CheckIn, bookingEntity.CheckOut);
        });
        
        if (BookingHelper.DateIntersects(booking.CheckIn, booking.CheckOut, existingBookings))
        {
            throw new InvalidOperationException("Listing is already booked for this period or a part of it.");
        }
        
        booking.TotalPrice = BookingHelper.CalculateTotalPrice(booking.CheckIn, booking.CheckOut, listingEntity.PricePerNight);
        
        return await _bookingCommandAccess.AddBooking(booking);
    }
    
}