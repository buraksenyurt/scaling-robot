using Librarian.Application.Common.Interfaces;
using Librarian.Data.Contexts;
using Microsoft.EntityFrameworkCore;
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
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddDbContext<LibrarianDbContext>(
                options => options.UseSqlite("Data Source=LibrarianDatabase.sqlite3")
                );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<LibrarianDbContext>());
            return services;
        }
    }
}
