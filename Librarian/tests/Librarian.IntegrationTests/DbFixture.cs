using Librarian.Data.Contexts;
using Librarian.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using System;
using System.IO;

namespace Librarian.IntegrationTests
{
    /*
     * Test uygulaması bir WebAPI değil ancak WebAPI çalışma zamanı bağımlılıklarını kullanması gerekiyor.
     * Örneğin DI mekanizmasını gibi ki Entity Db Context, Mediator gibi servisleri test çalışma zamanına alabilelim.
     */
    public class DbFixture
        : IDisposable
    {
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;
        public DbFixture()
        {
            // test projesinin appsettings dosyasını ConfigurationBuilder hizmetine alıyoruz.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build(); // Elimizde konfigurasyon ayarları mevcut artık

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();

            // Web Host çalışma zamanı ortamını DI servislerine mock olarak ekliyoruz
            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "Librarian.WebApi"));

            services.AddLogging(); // loglamayı ekledik
            startup.ConfigureServices(services);
            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<LibrarianDbContext>(); //EF context servisini çektik
            context.Database.Migrate(); // Migration işlemi başlatıldı
        }
        public void Dispose()
        {
        }
    }
}
