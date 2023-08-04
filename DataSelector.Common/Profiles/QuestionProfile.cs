using AutoMapper;
using DataSelector.Common.Dtos;
using DataSelector.Model.Models;

namespace DataSelector.Common.Profiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<QuestionDocument, QuestionResponseDto>()
            .ReverseMap();
    }
}