using System.Net;
using System.Web;
using Exchange.Common.OperationResult;
using Exchange.Common.OperationResult.Error;
using Exchange.Core.Pagination;
using Exchange.Domain.Enums;
using Exchange.Web.Blazor.Options;
using Exchange.Web.Blazor.Services.Transactions.Dto;
using Exchange.Web.Resources;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Exchange.Web.Blazor.Services.Transactions;

public class TransactionsService : BaseAuthService
{
    public TransactionsService(
        IHttpClientFactory httpClientFactory,
        ProtectedLocalStorage localStorage
    ) : base(httpClientFactory, localStorage)
    {
    }

    public async Task<OperationResult<PagedResult<TransactionBlazorDto>>?> GetTransactionsAsync(
        TransactionBlazorQueryDto transactionBlazorQueryDto, CancellationToken cancellationToken = default)
    {
        const string basePath = "api/v1/transactions";

        try
        {
            var client = HttpClientFactory.CreateClient(BlazorOptions.ClientName);

            await SetAuthorizationHeaderAsync(client);

            var queryParameters = HttpUtility.ParseQueryString(string.Empty);

            if (transactionBlazorQueryDto.Count.HasValue)
                queryParameters["count"] = transactionBlazorQueryDto.Count.Value.ToString();

            if (transactionBlazorQueryDto.Offset.HasValue)
                queryParameters["offset"] = transactionBlazorQueryDto.Offset.Value.ToString();

            if (transactionBlazorQueryDto.UserId.HasValue)
                queryParameters["userId"] = transactionBlazorQueryDto.UserId.Value.ToString();

            if (transactionBlazorQueryDto.TransactionId.HasValue)
                queryParameters["transactionId"] = transactionBlazorQueryDto.TransactionId.Value.ToString();

            if (transactionBlazorQueryDto.States?.Any() == true)
            {
                queryParameters["states"] = string.Join(",", transactionBlazorQueryDto.States);
            }

            if (transactionBlazorQueryDto.CreatedDateFrom.HasValue)
            {
                queryParameters["createdDateFrom"] = transactionBlazorQueryDto.CreatedDateFrom.Value.ToString("O");
            }

            if (transactionBlazorQueryDto.CreatedDateTo.HasValue)
            {
                queryParameters["createdDateTo"] = transactionBlazorQueryDto.CreatedDateTo.Value.ToString("O");
            }

            var uriBuilder = new UriBuilder(client.BaseAddress + basePath)
            {
                Query = queryParameters.ToString()
            };

            return await client.GetFromJsonAsync<OperationResult<PagedResult<TransactionBlazorDto>>>(uriBuilder.Uri,
                cancellationToken);
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

    public async Task<OperationResult<TransactionBlazorDto>?> GetTransactionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        const string basePath = "api/v1/transactions";

        try
        {
            var client = HttpClientFactory.CreateClient(BlazorOptions.ClientName);

            await SetAuthorizationHeaderAsync(client);

            var uriBuilder = new UriBuilder(client.BaseAddress + basePath + $"/{id}");

            return await client.GetFromJsonAsync<OperationResult<TransactionBlazorDto>?>(
                uriBuilder.Uri,
                cancellationToken);
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

    public async Task<EmptyOperationResult?> UpdateStateAsync(
        Guid id,
        TransactionState state,
        CancellationToken cancellationToken = default
    )
    {
        const string basePath = "api/v1/transactions";

        try
        {
            var client = HttpClientFactory.CreateClient(BlazorOptions.ClientName);

            await SetAuthorizationHeaderAsync(client);

            var uriBuilder = new UriBuilder(client.BaseAddress + basePath + $"/{id}");

            var payload = new { Status = state };

            var responseMessage = await client.PutAsJsonAsync(uriBuilder.Uri, payload, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                return new OperationError(responseMessage.StatusCode.ToString(), errorContent);
            }

            return await responseMessage.Content.ReadFromJsonAsync<EmptyOperationResult>(
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