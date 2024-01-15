using Domus.Common.Constants;
using Domus.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Domus.Common.Helpers;

public static class DataAccessHelper
{
	private static IConfiguration _configuration;
	private static IConfiguration Configuraiton
	{
		get => _configuration ?? throw new Exception("Configuration is not initialized");
		
		set => _configuration = value;
	}

	public static void InitConfiguration(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public static string GetConnectionString(string connectionName) => Configuraiton.GetConnectionString(connectionName) ?? throw new MissingConnectionStringException("Cannot find the specified connection string");

    public static string GetDefaultConnectionString() => GetConnectionString(DataAccessConstants.DEFAULT_CONNECTION_NAME);
}
