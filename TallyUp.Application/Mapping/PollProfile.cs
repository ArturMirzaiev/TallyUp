using AutoMapper;
using TallyUp.Application.Dtos;
using TallyUp.Domain.Entities;

namespace TallyUp.Application.Mapping;

public class PollProfile : Profile
{
    public PollProfile()
    {
        CreateMap<Poll, PollDto>();

        CreateMap<CreatePollDto, Poll>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())  
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdatePollDto, Poll>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())  
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)); // Теперь `src` явно указан
    }   
}