using FacadeMaster.Core.Settings;
using FacadeMaster.UI.Dialogs;
using Rhino;
using Rhino.Commands;
using Rhino.UI;

namespace FacadeMaster.RH.FM_Command
{
    /// <summary>
    /// 打开“本地数据位置”设置对话框：设置项目库 / 物料库的 JSON 文件路径。
    /// 保存后立即生效（不需要重启 Rhino），同时写入 %AppData%\FacadeMaster\settings.json。
    /// </summary>
    public class FMSettingsCommand : Command
    {
        public static FMSettingsCommand Instance { get; private set; }

        public FMSettingsCommand()
        {
            Instance = this;
        }

        public override string EnglishName => "FMSettings";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var settingsService = RhinoPlugin.Instance?.GetService<FMSettingsService>();
            if (settingsService == null)
            {
                RhinoApp.WriteLine("FacadeMaster 插件服务未初始化，无法打开设置。");
                return Result.Failure;
            }

            var dialog = new FMSettingsDialog(settingsService);
            dialog.ShowModal(RhinoEtoApp.MainWindowForDocument(doc));

            RhinoApp.WriteLine("FacadeMaster：项目库 {0}；物料库 {1}",
                settingsService.ProjectLibraryPath,
                settingsService.MaterialLibraryPath);

            return Result.Success;
        }
    }
}
