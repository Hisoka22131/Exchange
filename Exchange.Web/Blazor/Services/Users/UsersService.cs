using System.Net;
using System.Net.Http.Headers;
using System.Web;
using Exchange.Common.OperationResult;
using Exchange.Core.Pagination;
using Exchange.Web.Blazor.Options;
using Exchange.Web.Blazor.Services.Users.Dto;
using Exchange.Web.Resources;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Exchange.Web.Blazor.Services.Users;

public class UsersService : BaseAuthService
{
    public UsersService(
        IHttpClientFactory httpClientFactory,
        ProtectedLocalStorage localStorage
    ) : base(httpClientFactory, localStorage)
    {
    }

    public async Task<OperationResult<PagedResult<UserBlazorDto>>?> GetUsersAsync(
        UserBlazorQueryDto userBlazorQueryDto, CancellationToken cancellationToken = default)
    {
        const string basePath = "api/v1/users";

        try
        {
            var client = HttpClientFactory.CreateClient(BlazorOptions.ClientName);

            await SetAuthorizationHeaderAsync(client);

            var queryParameters = HttpUtility.ParseQueryString(string.Empty);

            if (userBlazorQueryDto.Count.HasValue)
                queryParameters["count"] = userBlazorQueryDto.Count.Value.ToString();

            if (userBlazorQueryDto.Offset.HasValue)
                queryParameters["offset"] = userBlazorQueryDto.Offset.Value.ToString();

            if (userBlazorQueryDto.UserId.HasValue)
                queryParameters["userId"] = userBlazorQueryDto.UserId.Value.ToString();

            if (!string.IsNullOrWhiteSpace(userBlazorQueryDto.TelegramUserName))
                queryParameters["telegramUserName"] = userBlazorQueryDto.TelegramUserName;

            var uriBuilder = new UriBuilder(client.BaseAddress + basePath)
            {
                Query = queryParameters.ToString()
            };

            return await client.GetFromJsonAsync<OperationResult<PagedResult<UserBlazorDto>>>(uriBuilder.Uri,
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
}