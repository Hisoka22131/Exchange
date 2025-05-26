using System.Net;
using Exchange.Common.OperationResult;
using Exchange.Web.Blazor.Login.Dto;
using Exchange.Web.Blazor.Options;
using Exchange.Web.Resources;

namespace Exchange.Web.Blazor.Login;

public class LoginService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<OperationResult<LoginResponseDto>?> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
    {
        const string basePath = "api/v1/login";

        try
        {
            var client = _httpClientFactory.CreateClient(BlazorOptions.ClientName);

            var uriBuilder = new Uri(client.BaseAddress + basePath);
            
            var httpResponse = await client.PostAsJsonAsync(
                requestUri: uriBuilder,
                value: loginRequestDto,
                options: null,
                cancellationToken:  cancellationToken);
            
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"{httpResponse.StatusCode} {httpResponse.ReasonPhrase}");
            }
            
            return await httpResponse.Content.ReadFromJsonAsync<OperationResult<LoginResponseDto>>(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return ApiErrors.Exception(ex.Message);
        }
    }
}