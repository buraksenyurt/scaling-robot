using Librarian.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Librarian.WebApi
{
    public class Program
    {
        /*
         * Serilog mekanizması Fluet API formasyonuna uygun bir kullanım şekli sunuyor. 
         * Bir metot zinciri olduğunu rahatlıkla görebiliyoruz.
         * LoggerConfiguration fonksiyonu ile nesne oluşturulduktan sonra farklı fonksiyonları çağırarak loglayıcıyı niteliklendiriyoruz.
         * 
         * Üç farklı ortama loglama yapıyoruz. SQLite, Json bazlı text dosya ve Console ekranı. Tabii bunların hepsi şart değil.
         * 
         * Log kayıtları standart olması açısından uygulamanın çalıştığı klasör altında Logs isimli bir başka klasörde toplanıyor.
         * 
         * Kitabın 18nci bölümünde SQLite yerine SQL Server'a göç söz konusu. Buna göre Main metodu değiştiriliyor ve Seed fonksiyonelliği ilave edilyor.
         */
        public static async Task<int> Main(string[] args)
        {
            var name = Assembly.GetExecutingAssembly().GetName();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", $"{name.Name}")
                .Enrich.WithProperty("Assembly", $"{name.Version}")
                .WriteTo.File(
                        new CompactJsonFormatter(),
                        Path.Combine(Environment.CurrentDirectory, "Logs", "log.json"),
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Information
                        )
                .WriteTo.SQLite(
                        Path.Combine(Environment.CurrentDirectory, "Logs", "log.db"),
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        storeTimestampInUtc: true
                        )
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Sunucu başlatılıyor");
                var host = CreateHostBuilder(args).Build();
                await RunDbMigrationAsync(host);
                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Ups! Beklenmedik bir hata oluştu.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task RunDbMigrationAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            /*
             * Servis Context'i üstünden Provider yakalanıyor.
             * Bundan faydalanarak DbContext nesnesini yakalıyoruz.
             * Eğer kullanılan veritabanı SqlServer ise migration işlemini çalıştırmaktayız.
             * Arkasından Seed fonksiyonunu çağırarak veri girişlerini bu DbContext örneği üstünden yaptırmaktayız.
             * Tabii olası bir exception alma ihtimali sebebiyle bir try catch kullanımı söz konusu.
             */
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<LibrarianDbContext>();
                if (context.Database.IsSqlServer())
                    await context.Database.MigrateAsync();
                await LibrarianDbContextSeed.SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Veritabanı seed işlemi sırasında bir hata oluştu");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Serilog özelliğini çalışma zamanına kazandırmak için eklemeliyiz.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
