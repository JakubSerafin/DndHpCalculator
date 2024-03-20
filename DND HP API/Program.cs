using System.Reflection;
using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;
using DND_HP_API.Controllers.Middleware;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Repositories;
using DND_HP_API.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

namespace DND_HP_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
        }).AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, _ => { });;
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
            var key = new OpenApiSecurityScheme()
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
            .AddScoped<ApiKeyAuthenticationHandler>();
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