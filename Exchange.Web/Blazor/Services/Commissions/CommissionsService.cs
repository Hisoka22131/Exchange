using System.Net;
using Exchange.Common.OperationResult;
using Exchange.Web.Blazor.Options;
using Exchange.Web.Blazor.Services.Commissions.Dto;
using Exchange.Web.Resources;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Exchange.Web.Blazor.Services.Commissions;

public class CommissionsService : BaseAuthService
{
    public CommissionsService(
        IHttpClientFactory httpClientFactory,
        ProtectedLocalStorage localStorage
    ) : base(httpClientFactory, localStorage)
    {
    }

    public async Task<OperationResult?> UpdateCommissionsAsync(
        IList<CommissionBlazorDto> commissionBlazorDtos,
        CancellationToken cancellationToken = default
    )
    {
        const string basePath = "api/v1/commissions";

        var response = new EmptyOperationResult();

        try
        {
            var client = HttpClientFactory.CreateClient(BlazorOptions.ClientName);

            await SetAuthorizationHeaderAsync(client);

            var uriBuilder = new UriBuilder(client.BaseAddress + basePath);

            var httpResponse = await client.PutAsJsonAsync(
                requestUri: uriBuilder.Uri,
                value: commissionBlazorDtos,
                options: null,
                cancellationToken: cancellationToken);

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{httpResponse.StatusCode} {httpResponse.ReasonPhrase}");
            }

            return await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResult>(cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await LocalStorage.DeleteAsync("authToken");
            }

            response.AddError(ApiErrors.Unauthorized);
            
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            
            response.AddError(ApiErrors.Exception(ex.Message));
            return response;
        }
    }
    
    public async Task<OperationResult<IList<CommissionBlazorDto>>?> GetCommissionsAsync(CancellationToken cancellationToken = default)
    {
        const string basePath = "api/v1/commissions";

        try
        {
            var client = HttpClientFactory.CreateClient(BlazorOptions.ClientName);

            await SetAuthorizationHeaderAsync(client);

            var uriBuilder = new UriBuilder(client.BaseAddress + basePath);

            return await client.GetFromJsonAsync<OperationResult<IList<CommissionBlazorDto>>>(
                requestUri: uriBuilder.Uri,
                options: null,
                cancellationToken: cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await LocalStorage.DeleteAsync("authToken");
            }

            return ApiErrors.Unauthorized;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return ApiErrors.Exception(ex.Message);
        }
    }
}