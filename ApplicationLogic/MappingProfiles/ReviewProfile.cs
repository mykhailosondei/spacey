using ApplicationCommon.DTOs.Review;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<ReviewCreateDTO, ReviewDTO>();
        CreateMap<ReviewUpdateDTO, ReviewDTO>();
        CreateMap<ReviewDTO, Review>().ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => src.Ratings.getRatingsArray()));
        CreateMap<Review, ReviewDTO>().ForMember(dest => dest.Ratings, opt => opt.MapFrom(src => new Ratings(src.Ratings)));
    }
}