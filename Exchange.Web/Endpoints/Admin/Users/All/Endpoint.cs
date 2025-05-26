using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Users.Query.GetAll;
using Exchange.Core.Pagination;
using Exchange.Web.Dto;
using MediatR;

namespace Exchange.Web.Endpoints.Admin.Users.All;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Users}";
    
    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet(Path, async (
                [FromQuery] int? count,
                [FromQuery] int? offset,
                [FromQuery] string? telegramUserName,
                [FromQuery] Guid? userId,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<PagedResult<UserDto>>();
                
                try
                {
                    var query = new GetAllUsersQuery(
                        Count: count, 
                        Offset: offset,
                        TelegramUserName: telegramUserName,
                        UserId: userId,
                        TelegramUserId: null,
                        IncludeTransactions: false);
                    
                    var usersPage = await mediator.Send(query, cancellationToken);

                    response.Result = new PagedResult<UserDto>
                    {
                        CurrentPage = usersPage.CurrentPage,     
                        PageSize = usersPage.PageSize,     
                        TotalCount = usersPage.TotalCount,     
                        Items = usersPage.Items.Select(x => new UserDto(x)) 
                    };
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("GetUsersByFilter")
            .WithDescription("Получить список пользователей по фильтру")
            .WithTags(Constants.Groups.Users)
            .RequireAuthorization("AdminPolicy")
            .WithOpenApi();

        return Task.CompletedTask;
    }
}