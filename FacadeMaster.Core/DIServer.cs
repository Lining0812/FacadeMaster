using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using FacadeMaster.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FacadeMaster.Core
{
    public static class DIExtensions
    {
        /// <summary>默认物料库 JSON 文件路径（用户未在 settings.json 里配置时使用）</summary>
        public static string MaterialLibraryPath => FMSettings.DefaultMaterialLibraryPath;

        /// <summary>默认项目库 JSON 文件路径（用户未在 settings.json 里配置时使用）</summary>
        public static string ProjectLibraryPath => FMSettings.DefaultProjectLibraryPath;

        /// <summary>用 settings.json 里配置的路径注册服务（文件不存在时自动生成默认配置）</summary>
        public static IServiceCollection AddFMServices(this IServiceCollection services)
        {
            return AddFMServices(services, FMSettings.Reload());
        }

        /// <summary>用指定的配置注册服务，便于测试或宿主自己决定配置来源</summary>
        public static IServiceCollection AddFMServices(this IServiceCollection services, FMSettings settings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            // 配置实例本身也进容器：界面改完路径写入的是同一个对象
            services.AddSingleton<FMSettings>(settings);

            services.AddSingleton<IProjectRepo>(_ => new ProjectRepository(settings.ResolvedProjectLibraryPath));
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<ProjectManager>();

            services.AddSingleton<IMaterialRepo>(_ => new MaterialRepository(settings.ResolvedMaterialLibraryPath));
            services.AddSingleton<IMaterialService, MaterialService>();

            // 运行期修改库文件位置（Rhino 的 FMSettings 命令 / 面板按钮使用）
            services.AddSingleton<FMSettingsService>();

            return services;
        }
    }
}
