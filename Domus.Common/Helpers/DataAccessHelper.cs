using System.Reflection;
using DbUp;
using Domus.Common.Constants;
using Domus.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Domus.Common.Helpers;

public static class DataAccessHelper
{
	private static IConfiguration _configuration;
	private static IConfiguration Configuration
	{
		get => _configuration ?? throw new Exception("Configuration is not initialized");
		
		set => _configuration = value;
	}

	public static void InitConfiguration(IConfiguration configuration)
	{	
		_configuration = configuration;
	}

	public static string GetConnectionString(string connectionName) => Configuration.GetConnectionString(connectionName) ?? throw new MissingConnectionStringException("Cannot find the specified connection string");

    public static string GetDefaultConnectionString() => GetConnectionString(DataAccessConstants.DEFAULT_CONNECTION_NAME);

    public static void EnsureMigrations(string assemblyName, string? connection = null)
    {
	    connection ??= GetDefaultConnectionString();
	    EnsureDatabase.For.SqlDatabase(connection);
        
	    var upgradeEngine = DeployChanges.To.SqlDatabase(connection)
		    .WithScriptsEmbeddedInAssembly(Assembly.Load(assemblyName))
		    .LogToConsole()
		    .Build();
	    var scripts = upgradeEngine.GetDiscoveredScripts();
	    if (scripts.Any())
	    {
		    upgradeEngine.PerformUpgrade();
	    }
	    else
	    {
		    Console.WriteLine("No scripts found");
	    }
    }
}
