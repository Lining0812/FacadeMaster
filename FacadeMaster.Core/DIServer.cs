using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FacadeMaster.Core
{
    public static class DIExtensions
    {
        public static IServiceCollection AddFMServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepo, ProjectRepository>();
            services.AddScoped<IProjectService,ProjectService>();

            return services;
        }
    }
}
