using System.Text.Json.Serialization;
using Domus.Api.Extensions;
using Domus.Api.Hub;
using Domus.Common.Constants;
using Domus.Common.Helpers;
using Domus.Domain.Entities;
using NLog;

LogManager.Setup()
    .LoadConfigurationFromFile($"{Directory.GetCurrentDirectory()}/Configurations/nlog.config")
    .GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);
DataAccessHelper.InitConfiguration(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDefaultCorsPolicy(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.AddGgAuthentication(builder.Configuration);
builder.Services.AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(CorsConstants.APP_CORS_POLICY);
app.UseHttpsRedirection();  
app.UseAuthentication();
app.UseAuthorization();

    app.MapHub<NotificationHub>("/notification");

app.MapControllers();

DataAccessHelper.EnsureMigrations(AppDomain.CurrentDomain.FriendlyName);
app.Run();

LogManager.Shutdown();
