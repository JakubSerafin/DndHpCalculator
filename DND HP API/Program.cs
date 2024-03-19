using System.Reflection;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Repositories;
using DND_HP_API.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DND_HP_API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<ICharacterSheetRepository, CharacterSheetInMemoryRepository>();
        builder.Services.AddSingleton<IHpModifierRepository, HpModifierInMemoryRepository>();
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        

        app.MapControllers();

        app.Run();
    }
}