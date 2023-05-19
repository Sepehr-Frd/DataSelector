using System.Text.Json;
using AmazonMockup.Common.Dtos;
using AmazonMockup.DataAccess;
using AmazonMockup.Model.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonMockup.ExternalService.RabbitMQ.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviceScopeFactory = serviceScopeFactory;

        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEventType(message);

        if (eventType is EventType.QuestionPublished)
        {

        }


    }

    private static EventType DetermineEventType(string notificationMessage)
    {
        var eventDto = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        return eventDto?.Event switch
        {
            "Question_Published" => EventType.QuestionPublished,
            _ => EventType.Undetermined
        };

    }

    private async Task AddQuestionAsync(string questionPublishedMessage, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IBaseRepository<QuestionDocument>>();
        
        var questionPublishedDto = JsonSerializer.Deserialize<QuestionPublishedDto>(questionPublishedMessage);

        try
        {
            var questionDocument = _mapper.Map<QuestionDocument>(questionPublishedDto!.QuestionResponseDto);
            // TODO: Check if the question already exists
            await repository.CreateOneAsync(questionDocument, cancellationToken);

        }
        catch (Exception exception)
        {
            // TODO: Add logging

            Console.WriteLine($"Could not add question document to database due to an exception: {exception.Message}");
        }




    }
}

