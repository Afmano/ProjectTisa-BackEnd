using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NReco.Logging.File;
using ProjectTisa.Controllers.GeneralData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<MainDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<RouteConfig>(builder.Configuration.GetSection("MyConfig"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(conf =>
{
    conf.AddConsole();
    conf.AddFile(builder.Configuration.GetSection("Logging"));
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    Exception? exception = context.Features
        ?.Get<IExceptionHandlerPathFeature>()
        ?.Error;
    app.Logger.LogError(exception, "Intercepted error.");
    await context.Response.WriteAsJsonAsync(ResAnswers.Error);
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("App is starting...");
app.Run();
