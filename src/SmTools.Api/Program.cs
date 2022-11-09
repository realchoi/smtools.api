using SmTools.Api;
using SpringMountain.Modularity;

/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();*/


Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices(services =>
        {
            services.ConfigureServiceCollection<StartupModule>();
        })
        .Configure(app =>
        {
            app.BuildApplicationBuilder();
        });
    })
    .Build()
    .Run();