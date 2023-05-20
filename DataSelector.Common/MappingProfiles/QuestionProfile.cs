using DataSelector.Common.Dtos;
using DataSelector.Model.Models;
using AutoMapper;

namespace DataSelector.Common.MappingProfiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<QuestionDocument, QuestionResponseDto>()
            .ReverseMap();

        CreateMap<QuestionPublishedDto, QuestionDocument>();
            
    }


}

