using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domus.Api.Settings;
using Domus.Common.Exceptions;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
using Domus.Service.Constants;
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
            Expires = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenLifetimeInMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    } 
}