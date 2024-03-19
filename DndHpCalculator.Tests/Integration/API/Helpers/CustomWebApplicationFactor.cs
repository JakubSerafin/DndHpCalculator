using DND_HP_API;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Repositories;
using DND_HP_API.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace DndHpCalculator.Tests.Integration.API.Helpers;

class CustomWebApplicationFactor : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //This class will be used to integration test of the API, so we must ensure that data is whipped clean between tests
        builder.ConfigureTestServices(services =>
        {
            ReinitializeClass<ICharacterSheetRepository, CharacterSheetInMemoryRepository>(services);
            ReinitializeClass<IHpModifierRepository, HpModifierInMemoryRepository>(services);
        });
        
        void ReinitializeClass<T, T2>(IServiceCollection services) where T:class where T2 : class, T
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton<T, T2>();
        }
    }
}