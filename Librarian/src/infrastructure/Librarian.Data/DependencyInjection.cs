using Librarian.Application.Common.Interfaces;
using Librarian.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Librarian.Data
{
    /*
     * 
     * Çalışma zamanı DI servislerini Entity Framework DbContext türevimizi eklemek için kullanılan sınıf.
     * 
     * Neden AddScoped kullandık?
     * 
     */
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services,IConfiguration configuration)
        {
            //services.AddDbContext<LibrarianDbContext>(
            //    options => options.UseSqlite(configuration.GetConnectionString("LibrarianDbConnection")) // SQLite veri tabanı bağlantı bilgisi konfigurasyon üstünden gelecek. Web API' nin appSettings.json dosyasından
            //    );

            services.AddDbContext<LibrarianDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("LibrarianDbConnection"))
                );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<LibrarianDbContext>());
            return services;
        }
    }
}
