using System.Security.Claims;
using System.Text;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Terminator.Application.Common;
using Terminator.Core.Entities;
using Terminator.Infrastructure.Common.Options;

namespace Terminator.Infrastructure.Services;

public class JwtProvider : IJwtProvider
{
    private readonly AuthOptions _options;

    public JwtProvider(IOptions<AuthOptions> authOptions)
    {
        _options = authOptions.Value;

        Guard.Against.NullOrWhiteSpace(_options.SecretKey);
    }

    public string Generate(User user)
    {
        var handler = new JsonWebTokenHandler();

        var key = new SymmetricSecurityKey(Convert.FromBase64String(_options.SecretKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Username)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_options.ExpirationDays),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = creds
        };

        return handler.CreateToken(tokenDescriptor);
    }
}