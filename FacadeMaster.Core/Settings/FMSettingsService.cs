using FacadeMaster.Core.Repos;
using System;
using System.Linq;

namespace FacadeMaster.Core.Settings
{
    /// <summary>
    /// 本地库位置设置的应用器：校验用户输入、写回 settings.json、
    /// 切换项目库 / 物料库仓储的文件并刷新内存数据。
    /// <para>
    /// 界面（Rhino 设置对话框）只跟这个类打交道，不要自己改 <see cref="FMSettings"/> 或
    /// <c>DIExtensions</c> 里的路径。
    /// </para>
    /// </summary>
    public class FMSettingsService
    {
        private readonly FMSettings _settings;
        private readonly IProjectRepo _projectRepo;
        private readonly IMaterialRepo _materialRepo;
        private readonly ProjectManager _projectManager;

        public FMSettingsService(
            FMSettings settings,
            IProjectRepo projectRepo,
            IMaterialRepo materialRepo,
            ProjectManager projectManager)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _projectRepo = projectRepo ?? throw new ArgumentNullException(nameof(projectRepo));
            _materialRepo = materialRepo ?? throw new ArgumentNullException(nameof(materialRepo));
            _projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
        }

        /// <summary>当前生效的配置（只读用途，修改请走 <see cref="Apply"/>）</summary>
        public FMSettings Current => _settings;

        /// <summary>当前项目库实际使用的文件全路径</summary>
        public string ProjectLibraryPath =>
            (_projectRepo as IFileRepository)?.FilePath ?? _settings.ResolvedProjectLibraryPath;

        /// <summary>当前物料库实际使用的文件全路径</summary>
        public string MaterialLibraryPath =>
            (_materialRepo as IFileRepository)?.FilePath ?? _settings.ResolvedMaterialLibraryPath;

        /// <summary>
        /// 应用新的库文件位置：先校验，再切换仓储（新文件不存在时把现有数据写过去），
        /// 最后写配置文件并刷新 ProjectManager 的项目列表。
        /// </summary>
        /// <exception cref="InvalidOperationException">路径为空、非法或两个库指向同一个文件</exception>
        public void Apply(FMSettings desired)
        {
            if (desired == null) throw new ArgumentNullException(nameof(desired));

            var projectPath = FMSettings.ResolveFilePath(desired.ProjectLibraryPath, FMSettings.ProjectLibraryFileName);
            var materialPath = FMSettings.ResolveFilePath(desired.MaterialLibraryPath, FMSettings.MaterialLibraryFileName);

            if (projectPath == null)
                throw new InvalidOperationException("项目库文件路径无效，请重新选择。");
            if (materialPath == null)
                throw new InvalidOperationException("物料库文件路径无效，请重新选择。");
            if (string.Equals(projectPath, materialPath, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("项目库与物料库不能使用同一个文件。");

            // 先切仓储：切换过程可能抛异常（目标文件被占用、无写权限等），此时配置保持不变
            if (_projectRepo is IFileRepository projectFileRepo)
                projectFileRepo.ChangeFilePath(projectPath);
            if (_materialRepo is IFileRepository materialFileRepo)
                materialFileRepo.ChangeFilePath(materialPath);

            _settings.ProjectLibraryPath = projectPath;
            _settings.MaterialLibraryPath = materialPath;
            _settings.Save();

            // 项目库换了文件，内存里的项目列表要跟着换；尽量保留原来选中的项目
            _projectManager.ReloadProjects(_projectManager.ActiveProject?.Id);
        }
    }
}
