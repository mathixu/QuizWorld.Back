using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// Represents the JWT provider.
/// </summary>
public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    /// <inheritdoc />
    public string Generate(User user)
    {
        var claims = GetClaims(user);

        return WriteToken(claims);
    }
     
    /// <inheritdoc />
    private string WriteToken(Claim[] claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(_options.AccessTokenExpireInMinutes
            ),
            signingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    private Claim[] GetClaims(User user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];
    }
}
