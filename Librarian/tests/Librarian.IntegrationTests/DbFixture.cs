using Librarian.Data.Contexts;
using Librarian.WebApi;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using System;
using System.IO;
using System.Threading.Tasks;

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
                TablesToIgnore = new[] { "__EFMigrationsHistory" } // Respawn paketindeki Checkpoint nesnesini oluştururken bu tabloyu dikkate almamasını belirtmiş olduk
            };
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<LibrarianDbContext>(); //EF context servisini çektik
            context.Database.Migrate(); // Migration işlemi başlatıldı
        }
        // Mediator'a talep göndermek için.
        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope(); // Scope oluşturuluyor
            var mediator = scope.ServiceProvider.GetService<IMediator>(); // O scope'tan IMediator implementasyonunu alıyoruz
            return await mediator.Send(request); // Gelen servis talebini gönderiyoruz
        }

        // Yeni bir Entity nesnesini veritabanına eklemek için.
        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope(); // Scope oluşturuluyor
            var context = scope.ServiceProvider.GetService<LibrarianDbContext>(); // Oluşturulan Scope'dan EF servis nesnesi alınıyor
            context.Add(entity); // Gelen entity veritabanına ekleniyor
            await context.SaveChangesAsync(); // Kayıt
        }

        // Test veritabanını ilk konumuna almak için
        // Respawn ile gelen Checkpoint nesnesi yardımıyla konfigurasyondaki veritabanı bağlantısı için bu işlem gerçeklenir.
        public static async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("LibrarianDbConnection"));
        }

        public static async Task<TEntity> FindAsync<TEntity>(int id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<LibrarianDbContext>();
            return await context.FindAsync<TEntity>(id);
        }

        public void Dispose()
        {
        }
    }
}
