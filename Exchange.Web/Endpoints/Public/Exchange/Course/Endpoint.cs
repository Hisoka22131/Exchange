using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Mediator.Course.Query;
using MediatR;

namespace Exchange.Web.Endpoints.Public.Exchange.Course;

internal sealed class Endpoint : IEndpoint
{
    private const string Path = $"/api/v1/{Constants.Groups.Exchanges}/course";

    public Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost(Path, async (
                [FromBody] PostCourseRequest req,
                IMediator mediator,
                CancellationToken cancellationToken = default
            ) =>
            {
                var response = new OperationResult<Domain.Entities.Course>();
                try
                {
                    var request = new GetCourseQuery(
                        FromCurrency: req.CurrencyFrom,
                        FromAmount: req.AmountFrom,
                        ToCurrency: req.CurrencyTo,
                        ToAmount: req.AmountTo);

                    response.Result = await mediator.Send(request, cancellationToken);
                    
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                    response.AddError(CommonErrors.CommonError(ex.Message));

                    return Results.Ok(response);
                }
            })
            .WithName("ExchangeCourse")
            .WithDescription("Получить курс")
            .WithTags(Constants.Groups.Exchanges)
            .WithOpenApi();

        return Task.CompletedTask;
    }
}