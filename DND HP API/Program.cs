using DND_HP_API.Controllers.Middleware;
using DND_HP_API.Domain.Repositories;
using DND_HP_API.Infrastructure;
using DnDHpCalculator.Database;
using Microsoft.OpenApi.Models;

namespace DND_HP_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var apiKey = configuration["ApiKey"];
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
#pragma warning disable CS0618 // Type or member is obsolete
        }).AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
#pragma warning restore CS0618 // Type or member is obsolete
            ApiKeyAuthenticationOptions.DefaultScheme,
            options => { options.ApiKey = apiKey; });
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "ApiKey must appear in header",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var key = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
            {
                { key, new List<string>() }
            };
            c.AddSecurityRequirement(requirement);
        });
        builder.Services.AddSingleton<ICharacterSheetRepository, CharacterSheetSqlLittleRepository>();
        builder.Services.AddSingleton<IHpModifierRepository, HpModifierInMemoryRepository>()
#pragma warning disable CS0618 // Type or member is obsolete
            .AddScoped<ApiKeyAuthenticationHandler>();
#pragma warning restore CS0618 // Type or member is obsolete
        var app = builder.Build();
        SqlLiteDatabase.Initialize();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        //app.UseMiddleware<ApiKeyMiddleware>("kupapupakonfacela");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}