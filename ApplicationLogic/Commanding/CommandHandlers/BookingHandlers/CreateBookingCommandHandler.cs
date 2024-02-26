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
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MongoDB.Driver.Linq;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class CreateBookingCommandHandler : BaseHandler, IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IPublisher _publisher;

    public CreateBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess, IListingQueryRepository listingQueryRepository, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter, IUserQueryRepository userQueryRepository, IPublisher publisher) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
        _listingQueryRepository = listingQueryRepository;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
        _userQueryRepository = userQueryRepository;
        _publisher = publisher;
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
        
        var bookings = await _bookingQueryRepository.GetBookingsByListingId(booking.ListingId);

        (DateTime CheckIn, DateTime CheckOut)[] existingBookings =
            bookings.Where(b => b.Status == BookingStatus.Active).Select(b => (b.CheckIn, b.CheckOut)).ToArray();
        
        if (BookingHelper.DateIntersects(booking.CheckIn, booking.CheckOut, existingBookings))
        {
            throw new InvalidOperationException("Listing is already booked for this period or a part of it.");
        }
        
        booking.TotalPrice = BookingHelper.CalculateTotalPrice(booking.CheckIn, booking.CheckOut, listingEntity.PricePerNight);
        
        await _publisher.Publish(new BookingCreatedEvent()
        {
            ListingId = booking.ListingId,
            UserId = booking.UserId,
            CreatedAt = DateTime.UtcNow
        }, CancellationToken.None);
        
        return await _bookingCommandAccess.AddBooking(booking);
    }
    
}