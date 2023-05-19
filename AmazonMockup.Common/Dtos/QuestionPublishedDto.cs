using System;
namespace AmazonMockup.Common.Dtos;

public class QuestionPublishedDto
{
    public QuestionPublishedDto(QuestionResponseDto questionResponseDto, string publishEvent)
    {
        QuestionResponseDto = questionResponseDto;

        Event = publishEvent;
    }

    public QuestionPublishedDto()
    {
    }

    public QuestionResponseDto? QuestionResponseDto { get; set; }

    public string? Event { get; set; }
}

