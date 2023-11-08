using ApplicationCommon.DTOs.Booking;
using ApplicationCommon.DTOs.Review;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<ReviewCreateDTO, ReviewDTO>();
        CreateMap<ReviewUpdateDTO, ReviewDTO>();
        CreateMap<ReviewDTO, Review>();
        CreateMap<Review, ReviewDTO>();
    }
}