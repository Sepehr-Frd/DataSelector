using AmazonMockup.Common.Dtos;
using AmazonMockup.Model.Models;
using AutoMapper;

namespace AmazonMockup.Common.MappingProfiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<QuestionDocument, QuestionResponseDto>()
            .ReverseMap();

        CreateMap<QuestionPublishedDto, QuestionDocument>();
            
    }


}

