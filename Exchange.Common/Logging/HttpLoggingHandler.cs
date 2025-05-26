using Microsoft.Extensions.Logging;

namespace Exchange.Common.Logging;

public class HttpLoggingHandler : DelegatingHandler
{
    private readonly ILogger<HttpLoggingHandler> _logger;

    public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending request to {Url}", request.RequestUri);

        if (request.Content != null)
        {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Request Content: {Content}", content);
        }

        var response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("Received response from {Url} with status code {StatusCode}", request.RequestUri, response.StatusCode);

        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Response Content: {Content}", responseContent);
        }

        return response;
    }
}
