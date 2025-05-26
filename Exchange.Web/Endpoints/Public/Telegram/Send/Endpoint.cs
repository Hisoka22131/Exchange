using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Messages.Commands;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Telegram.Send;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/{Constants.Groups.Telegram}/message";
    
    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(Path, async (
                object message,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new EmptyOperationResult();
                
                try
                {
                    ArgumentNullException.ThrowIfNull(message);
                    
                    var command = new HandleMessageCommand(message.ToString()!);

                    await mediator.Send(command, cancellationToken);

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("SendTelegramMessage")
            .WithDescription("Отправить сообщение в телеграмм")
            .WithTags(Constants.Groups.Telegram)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}