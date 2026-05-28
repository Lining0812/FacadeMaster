using FacadeMaster.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;

namespace FacadeMaster.Core.Settings
{
    /// <summary>
    /// FacadeMaster 的本地配置（本地库 JSON 文件的位置）。
    /// <para>
    /// 配置存放在 <c>%AppData%\FacadeMaster\settings.json</c>：用户既可以直接编辑该文件，
    /// 也可以在 Rhino 里运行 <c>FMSettings</c> 命令用界面修改。
    /// </para>
    /// <para>
    /// 路径留空表示使用默认位置（<c>%AppData%\FacadeMaster\Projects.json</c> /
    /// <c>%AppData%\FacadeMaster\MaterialLibrary.json</c>）。
    /// </para>
    /// </summary>
    public class FMSettings
    {
        /// <summary>配置文件名</summary>
        public const string SettingsFileName = "settings.json";
        /// <summary>项目库默认文件名</summary>
        public const string ProjectLibraryFileName = "Projects.json";
        /// <summary>物料库默认文件名</summary>
        public const string MaterialLibraryFileName = "MaterialLibrary.json";

        private static readonly object SyncRoot = new object();
        private static FMSettings _current;

        /// <summary>项目库文件路径（可为空，空表示用默认路径；也允许填目录）</summary>
        public string ProjectLibraryPath { get; set; }

        /// <summary>物料库文件路径（可为空，空表示用默认路径；也允许填目录）</summary>
        public string MaterialLibraryPath { get; set; }

        /// <summary>配置目录：%AppData%\FacadeMaster</summary>
        public static string AppDataDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FacadeMaster");

        /// <summary>配置文件全路径：%AppData%\FacadeMaster\settings.json</summary>
        public static string SettingsFilePath => Path.Combine(AppDataDirectory, SettingsFileName);

        /// <summary>项目库默认全路径</summary>
        public static string DefaultProjectLibraryPath => Path.Combine(AppDataDirectory, ProjectLibraryFileName);

        /// <summary>物料库默认全路径</summary>
        public static string DefaultMaterialLibraryPath => Path.Combine(AppDataDirectory, MaterialLibraryFileName);

        /// <summary>进程内缓存的当前配置（首次访问时从磁盘读取，读不到就用默认值）</summary>
        public static FMSettings Current
        {
            get { lock (SyncRoot) { return _current ?? (_current = Load()); } }
        }

        /// <summary>实际生效的项目库文件全路径</summary>
        [JsonIgnore]
        public string ResolvedProjectLibraryPath =>
            ResolveFilePath(ProjectLibraryPath, ProjectLibraryFileName) ?? DefaultProjectLibraryPath;

        /// <summary>实际生效的物料库文件全路径</summary>
        [JsonIgnore]
        public string ResolvedMaterialLibraryPath =>
            ResolveFilePath(MaterialLibraryPath, MaterialLibraryFileName) ?? DefaultMaterialLibraryPath;

        /// <summary>
        /// 从磁盘读取配置。文件不存在时按默认路径生成一份，
        /// 这样用户至少能在 %AppData%\FacadeMaster 下看到并编辑它。
        /// </summary>
        public static FMSettings Load()
        {
            if (!File.Exists(SettingsFilePath))
            {
                var defaults = new FMSettings
                {
                    ProjectLibraryPath = DefaultProjectLibraryPath,
                    MaterialLibraryPath = DefaultMaterialLibraryPath
                };
                // 写文件失败（只读目录等）不影响插件启动
                try { defaults.Save(); } catch { /* 忽略 */ }
                return defaults;
            }

            try
            {
                var settings = JsonHelper.Deserialize<FMSettings>(File.ReadAllText(SettingsFilePath));
                return settings ?? new FMSettings();
            }
            catch
            {
                // 配置文件被改坏时退回默认值，不能让插件起不来
                return new FMSettings();
            }
        }

        /// <summary>重新从磁盘读取并更新进程内缓存，返回最新配置</summary>
        public static FMSettings Reload()
        {
            lock (SyncRoot)
            {
                _current = Load();
                return _current;
            }
        }

        /// <summary>保存到 settings.json（保存前会把路径规范化成文件全路径）</summary>
        public void Save()
        {
            ProjectLibraryPath = ResolveFilePath(ProjectLibraryPath, ProjectLibraryFileName) ?? DefaultProjectLibraryPath;
            MaterialLibraryPath = ResolveFilePath(MaterialLibraryPath, MaterialLibraryFileName) ?? DefaultMaterialLibraryPath;

            lock (SyncRoot)
            {
                JsonHelper.SerializeToFile(this, SettingsFilePath);
                _current = this;
            }
        }

        /// <summary>复制一份，供界面在“确定”前编辑而不污染当前配置</summary>
        public FMSettings Clone()
        {
            return new FMSettings
            {
                ProjectLibraryPath = ProjectLibraryPath,
                MaterialLibraryPath = MaterialLibraryPath
            };
        }

        /// <summary>
        /// 把用户输入的路径规范成文件全路径：
        /// 去掉首尾空白与引号、展开 %环境变量%、填的是目录则补默认文件名、没有扩展名则补 .json。
        /// 输入为空返回 null。
        /// </summary>
        public static string ResolveFilePath(string input, string defaultFileName)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            var path = Environment.ExpandEnvironmentVariables(input.Trim().Trim('"'));
            if (string.IsNullOrWhiteSpace(path)) return null;

            try
            {
                if (Directory.Exists(path))
                    path = Path.Combine(path, defaultFileName);

                if (string.IsNullOrEmpty(Path.GetExtension(path)))
                    path += ".json";

                return Path.GetFullPath(path);
            }
            catch
            {
                // 非法字符 / 路径过长等，交给调用方按“路径无效”处理
                return null;
            }
        }
    }
}
