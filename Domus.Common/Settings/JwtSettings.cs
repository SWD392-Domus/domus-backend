namespace Domus.Api.Settings;

public class JwtSettings 
{
	public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SigningKey { get; init; }
    public bool ValidateAudience { get; init; }
    public bool ValidateIssuer { get; init; }
    public bool ValidateIssuerSigningKey { get; init; }
    public bool ValidateLifetime { get; init; }
	public int AccessTokenLifetimeInMinute { get; init; }
	public int RefreshTokenLifetimeInMinute { get; init; }
}
