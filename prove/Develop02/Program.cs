using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Develop02.Data;
using Develop02.Views;
using Develop02.Domain.Interfaces;
using Develop02.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<JournalContext>();
builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<IJournalRepository, JournalRepository>();
builder.Services.AddHostedService<MenuView>();

var host = builder.Build();

await host.StartAsync();

await host.StopAsync();

Console.WriteLine("Please, press enter to terminate the program...");
Console.ReadKey();