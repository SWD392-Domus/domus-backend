using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using Domus.Common.Constants;
using Domus.Common.Exceptions;
using Domus.Common.Settings;
using Domus.Service.Interfaces;
using Domus.Service.Models;
using Domus.Service.Models.Requests.Authentication;
using Domus.Service.Models.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Domus.Service.Implementations;

public class GoogleOAuthService : IGoogleOAuthService
{
	private readonly IConfiguration _configuration;

	public GoogleOAuthService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<ServiceActionResult> LoginAsync(OAuthRequest request)
	{
		var googleSettings = _configuration.GetSection(nameof(GoogleSettings)).Get<GoogleSettings>() ?? throw new MissingGoogleSettingsException();
		var googleLoginRequest = (GoogleLoginRequest)request;

		var client = new HttpClient();
		var code = WebUtility.UrlDecode(googleLoginRequest.Code);
		var requestParams = new Dictionary<string, string>
		{
			{ GoogleAuthConstants.CODE, code },
			{ GoogleAuthConstants.CLIENT_ID, googleSettings.ClientId },
			{ GoogleAuthConstants.CLIENT_SECRET, googleSettings.ClientSecret },
			{ GoogleAuthConstants.REDIRECT_URI, googleSettings.RedirectUri },
			{ GoogleAuthConstants.GRANT_TYPE, GoogleAuthConstants.AUTHORIZATION_CODE }
		};
		var content = new FormUrlEncodedContent(requestParams);
		var response = await client.PostAsync(GoogleAuthConstants.GOOGLE_TOKEN_URL, content);
		var authObject = JsonConvert.DeserializeObject<GoogleAuthResponse>(await response.Content.ReadAsStringAsync()) ?? throw new Exception("something went wrong");
		var handler = new JwtSecurityTokenHandler();
		var securityToken = handler.ReadJwtToken(authObject.IdToken);
		var googleJwt = "";

        return new ServiceActionResult(true) { Data = await response.Content.ReadAsStringAsync() };
    }
}
