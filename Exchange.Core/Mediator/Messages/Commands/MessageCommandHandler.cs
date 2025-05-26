using Exchange.Domain.Interfaces;
using MediatR;

namespace Exchange.Core.Mediator.Messages.Commands;

public class MessageCommandHandler : IRequestHandler<HandleMessageCommand>
{
    private readonly IMessageHandler _messageHandler;

    public MessageCommandHandler(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task Handle(HandleMessageCommand request, CancellationToken cancellationToken)
    {
        await _messageHandler.HandleMessageAsync(request.Message, cancellationToken);
    }
}