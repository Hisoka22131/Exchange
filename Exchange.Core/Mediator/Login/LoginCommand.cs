using MediatR;

namespace Exchange.Core.Mediator.Login;

public record LoginCommand(string Username, string Password) : IRequest<string>;