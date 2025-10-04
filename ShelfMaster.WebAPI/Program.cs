using ShelfMaster.WebAPI.Commons;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

builder.Services.AddApiService(builder.Configuration);


var app = builder.Build();

app.Run();
