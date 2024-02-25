using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domus.Api.Settings;
using Domus.Common.Exceptions;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Constants;
using Domus.Service.Exceptions;
using Domus.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domus.Service.Implementations;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JwtService(
        IConfiguration configuration,
        IUserTokenRepository userTokenRepository,
        IUnitOfWork unitOfWork
    )
    {
        _jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? throw new MissingJwtSettingsException();
        _userTokenRepository = userTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateRefreshToken(string userId)
    {
        var existingRefreshToken = await _userTokenRepository.GetAsync(token =>
            token.Name == TokenTypeConstants.REFRESH_TOKEN && token.LoginProvider == LoginProviderConstants.DOMUS_APP &&
            token.UserId == userId);
        if (existingRefreshToken is null)
        {
            existingRefreshToken = new IdentityUserToken<string>
            {
                Name = TokenTypeConstants.REFRESH_TOKEN,
                UserId = userId,
                LoginProvider = LoginProviderConstants.DOMUS_APP,
                Value = Guid.NewGuid().ToString()
            };
            await _userTokenRepository.AddAsync(existingRefreshToken);
        }
        else
        {
            existingRefreshToken.Value = Guid.NewGuid().ToString();
            await _userTokenRepository.UpdateAsync(existingRefreshToken);
        }
        
        await _unitOfWork.CommitAsync();
        return existingRefreshToken.Value;
    }

    public bool IsValidToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidAudience = _jwtSettings.Audience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            ValidateLifetime = _jwtSettings.ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public object? GetTokenClaim(string token, string claimName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidAudience = _jwtSettings.Audience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
            ValidateLifetime = _jwtSettings.ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        };
        
        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var jwtSecurityToken = (JwtSecurityToken)validatedToken;
            var propInfo = typeof(JwtSecurityToken).GetProperties().FirstOrDefault(p => p.Name == claimName);
            return propInfo?.GetValue(jwtSecurityToken);
        }
        catch
        {
            throw new InvalidTokenException();
        }
    }

    public string GenerateAccessToken(DomusUser user, IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            // Expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenLifetimeInMinutes),
            Expires = DateTime.Now.AddMinutes(60),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    } 
}