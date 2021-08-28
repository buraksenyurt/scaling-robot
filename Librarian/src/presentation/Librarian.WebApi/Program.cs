using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using System;
using System.IO;
using System.Reflection;

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
         */
        public static int Main(string[] args)
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
                CreateHostBuilder(args).Build().Run();
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Serilog özelliğini çalışma zamanına kazandırmak için eklemeliyiz.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
