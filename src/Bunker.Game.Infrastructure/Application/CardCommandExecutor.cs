using Bunker.Domain.Shared.Cards.CardActionCommands;
using Bunker.Game.Domain.AggregateModels.Characters.Cards;
using Microsoft.Extensions.DependencyInjection;

namespace Bunker.Game.Infrastructure.Application;

public class CardCommandExecutor : ICardCommandExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public CardCommandExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteCardAction(CardActionCommand command)
    {
        var commandType = command.GetType();

        var handlerType = typeof(ICardActionCommandHandler<>).MakeGenericType(commandType);

        var handler = _serviceProvider.GetRequiredService(handlerType);

        var method = handlerType.GetMethod(nameof(ICardActionCommandHandler<CardActionCommand>.Handle));

        if (method is null)
            throw new InvalidOperationException($"Method Handle not found in handler {handlerType.Name}");

        var task = (Task)method.Invoke(handler, new object[] { command, CancellationToken.None })!;

        await task;
    }
}
