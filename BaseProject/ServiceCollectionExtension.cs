using Repositories;
using Repositories.Base;
using Services;
using Services.Interface;

namespace Marketplace
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseGuidRepository<>), typeof(BaseGuidRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<DataContext>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
