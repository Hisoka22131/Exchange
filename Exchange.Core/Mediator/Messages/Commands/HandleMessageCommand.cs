using MediatR;

namespace Exchange.Core.Mediator.Messages.Commands;

public sealed record HandleMessageCommand(string Message) : IRequest;