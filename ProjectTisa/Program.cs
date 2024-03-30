using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NReco.Logging.File;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Exceptions;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models.Enums;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
IConfiguration appInfo = builder.Configuration.GetSection("AppInfo");
builder.Services.Configure<RouteConfig>(appInfo);
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Tisa API", Version = appInfo.GetValue<string>("Version") });
    setup.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<MainDbContext>(options => options.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMvc().AddJsonOptions(o => { 
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
});
builder.Services.AddLogging(conf =>
{
    conf.AddConsole();
    conf.AddFile(builder.Configuration.GetSection("Logging"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        RouteConfig appConfig = appInfo.Get<RouteConfig>() ?? throw new NullReferenceException("appsettings.json RouteConfig is null");
        EmailSender.Configure(appConfig.SmtpData);

        options.Authority = appConfig.CurrentHost;

        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = appConfig.AuthData.Issuer,
            ValidAudience = appConfig.AuthData.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfig.AuthData.IssuerSigningKey))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("admin", policy => policy.RequireRole(RoleType.SuperAdmin.ToString(), RoleType.Admin.ToString()))
    .AddPolicy("manage", policy => policy.RequireRole(RoleType.SuperAdmin.ToString(), RoleType.Admin.ToString(), RoleType.Manager.ToString()));

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
    await context.Response.WriteAsJsonAsync(exception is ControllerException ? exception.Message : ResAnswers.Error);
}));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("App is starting...");
app.Run();
