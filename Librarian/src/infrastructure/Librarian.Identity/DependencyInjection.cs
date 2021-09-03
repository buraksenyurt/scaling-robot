using Librarian.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Librarian.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AuthenticationSettings>(config.GetSection(nameof(AuthenticationSettings)));
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
