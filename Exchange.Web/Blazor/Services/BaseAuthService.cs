using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Exchange.Web.Blazor.Services;

public class BaseAuthService
{
    protected readonly IHttpClientFactory HttpClientFactory;
    protected readonly ProtectedLocalStorage LocalStorage;

    public BaseAuthService(IHttpClientFactory httpClientFactory, ProtectedLocalStorage localStorage)
    {
        HttpClientFactory = httpClientFactory;
        LocalStorage = localStorage;
    }
    
    protected async Task SetAuthorizationHeaderAsync(HttpClient client)
    {
        var token = (await LocalStorage.GetAsync<string>("authToken")).Value;

        if (string.IsNullOrWhiteSpace(token))
            return;
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}