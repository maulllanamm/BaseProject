using Repositories;
using Repositories.Base;
using Services;

namespace Marketplace
{
    public static class ServiceCollectionExtension
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<DataContext>();
            services.AddHttpContextAccessor();
        }
    }
}
