using ApplicationCommon.DTOs.Booking;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Review;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<BookingCreateDTO, BookingDTO>();
        CreateMap<BookingUpdateDTO, BookingDTO>();
        CreateMap<BookingDTO, Booking>()
            #region Mapping
            .AfterMap((src, dest, context) =>
            {
                if (src.Review == null)
                {
                    dest.Review = null;
                    return;
                }
                dest.Review = context.Mapper.Map<ReviewDTO, Review>(src.Review);
            })
            .ReverseMap()
            .AfterMap((src, dest, context) =>
            {
                if(src.Review == null)
                {
                    dest.Review = null;
                    return;
                }
                dest.Review = context.Mapper.Map<Review, ReviewDTO>(src.Review);
            });
            #endregion
    }
}