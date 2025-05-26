using Exchange.Common.OperationResult;
using Exchange.Core.Mediator.Login;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Login;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = "/api/v1/login";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(Path, async (
                [FromBody] LoginRequest req,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<LoginResponse>();

                var command = new LoginCommand(req.Username, req.Password);
                
                var token = await mediator.Send(command, cancellationToken);
                
                response.Result = new LoginResponse(token);
                
                return Results.Ok(response);
            })
            .WithName("AdminLogin")
            .WithDescription("Авторизация в админке")
            .WithTags("Admin")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}