using Exchange.Core.Helpers;
using Exchange.Core.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Exchange.Core.Mediator.Login;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly AuthOptions _authOptions;

    public LoginCommandHandler(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }
    
    public Task<string> Handle(LoginCommand req, CancellationToken cancellationToken)
    {
        var adminLogin = _authOptions.Username;
        var adminPassword = _authOptions.Password;
        var secretKey = _authOptions.SecretKey;

        if (!req.Username.Equals(adminLogin) || !req.Password.Equals(adminPassword))
        {
            return Task.FromResult(string.Empty);
        }

        var tokenString = new JwtTokenGenerator(secretKey)
            .GenerateToken(adminLogin, "Admin");

        return Task.FromResult(tokenString);
    }
}