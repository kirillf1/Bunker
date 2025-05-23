﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Bunker.Application.Shared.IntegrationEvents;

public class IntegrationEventLogEntry
{
    private static readonly JsonSerializerOptions s_indentedOptions = new() { WriteIndented = true };

    private static readonly JsonSerializerOptions s_caseInsensitiveOptions =
        new() { PropertyNameCaseInsensitive = true };

    public Guid EventId { get; private set; }

    [Required]
    public string EventTypeName { get; private set; }

    [NotMapped]
    public string EventTypeShortName => EventTypeName.Split('.')?.Last();

    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }
    public EventState State { get; set; }
    public int TimesSent { get; set; }
    public DateTime CreationTime { get; private set; }

    [Required]
    public string Content { get; private set; }
    public Guid TransactionId { get; private set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private IntegrationEventLogEntry() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public IntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreationDate;
        EventTypeName = @event.GetType().FullName!;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), s_indentedOptions);
        State = EventState.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
    }

    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, s_caseInsensitiveOptions) as IntegrationEvent;
        return this;
    }
}
