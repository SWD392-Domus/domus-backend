using AutoMapper;
using Domus.DAL.Implementations;
using Domus.Service.Implementations;
using Domus.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
namespace Domus.UnitTest;

public class DISetup
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static void InitializeServices()
    {
        var services = new ServiceCollection();
        services.AddScoped<IServiceService, ServiceService>();
        ServiceProvider = services.BuildServiceProvider();
    }

    public static T GetRequiredService<T>()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}

[TestFixture]
public class Tests
{
    
    [SetUp]
    public void Setup()
    {
        DISetup.InitializeServices();
    }

    [Test]
    public void TestCheckAllService()
    {

        // try
        // {
        //     var _serviceService = DISetup.GetRequiredService<IServiceService>();
        //     var idList = new List<Guid>()
        //     {
        //         new Guid("25df20fb-915b-449d-a265-08dc255f6e0f"),
        //         new Guid("cd0bf091-d0e8-4747-a266-08dc255f6e0f"),
        //     };
        //     Console.WriteLine(_serviceService.GetServices(idList));
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"Exception: {ex.Message}");
        //     throw; // Re-throw the exception to ensure the test fails
        // }
    }

}