using Librarian.Application.Common.Interfaces;
using Librarian.Domain.Settings;
using Librarian.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Librarian.Shared
{
    /*
     * 
     * Çağıran ortamdaki DI servislerine Shared tarafından gelen bağımlılıkları yüklememizi kolaylaştıran sınıf.
     * Librarian.Application'daki DependencyInjection sınıfı ile aynı teoriyi kullanıyor ve yine
     * kuvvetle muhtemel buradaki servisleri tüketecek olan Web API projesindeki Startup sınıfındaki ConfigureServices içinden çağırılacak
     */
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICsvBuilder, CsvBuilder>();

            return services;
        }
    }
}
