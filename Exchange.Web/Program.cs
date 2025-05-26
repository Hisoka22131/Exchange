using Exchange.Database.Context;
using Exchange.Web;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataProtection()
    .PersistKeysToDbContext<ExchangeDbContext>();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseServices();

app.Run();