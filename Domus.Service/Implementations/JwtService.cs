using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domus.Api.Settings;
using Domus.Common.Exceptions;
using Domus.DAL.Interfaces;
using Domus.Domain.Entities;
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
		var refreshToken = new IdentityUserToken<string>
        {
			Name = "REFRESH_TOKEN",
		 	UserId = userId,
		 	LoginProvider = "DOMUS_APP",
			Value = Guid.NewGuid().ToString()
        };

        await _userTokenRepository.AddAsync(refreshToken);
        await _unitOfWork.CommitAsync();
        return refreshToken.Value;
    }

    public string GenerateToken(DomusUser user, IEnumerable<string> roles)
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
            Expires = DateTime.Now.AddHours(_jwtSettings.AccessTokenLifetimeInMinute),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    } 
}
