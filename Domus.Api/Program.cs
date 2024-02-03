using Domus.Api.Extensions;
using Domus.Common.Constants;
using Domus.Common.Helpers;
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
var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

DataAccessHelper.EnsureMigrations(AppDomain.CurrentDomain.FriendlyName);
app.Run();

LogManager.Shutdown();
