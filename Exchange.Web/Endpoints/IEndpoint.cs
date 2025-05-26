namespace Exchange.Web.Endpoints;

public interface IEndpoint
{
    Task MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}