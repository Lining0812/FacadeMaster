using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.Core.Settings;
using System;
using System.IO;

namespace FacadeMaster.UI.Dialogs
{
    /// <summary>
    /// 本地数据位置设置对话框：让用户自己指定项目库 / 物料库的 JSON 文件位置。
    /// <para>
    /// 确定后由 <see cref="FMSettingsService"/> 写回 settings.json 并立即切换仓储文件，
    /// 不需要重启 Rhino。
    /// </para>
    /// </summary>
    public class FMSettingsDialog : Dialog
    {
        #region UI控件
        private readonly TextBox _txtProjectLibraryPath;
        private readonly TextBox _txtMaterialLibraryPath;
        private readonly Button _btnBrowseProject;
        private readonly Button _btnBrowseMaterial;
        private readonly Button _btnRestoreDefaults;
        private readonly Button _btnOk;
        private readonly Button _btnCancel;
        #endregion

        private readonly FMSettingsService _settingsService;

        public FMSettingsDialog(FMSettingsService settingsService)
        {
            if (settingsService == null) throw new ArgumentNullException(nameof(settingsService));
            _settingsService = settingsService;

            Title = "FacadeMaster 设置 - 本地数据位置";
            Resizable = true;
            MinimumSize = new Size(640, 240);

            _txtProjectLibraryPath = new TextBox();
            _txtMaterialLibraryPath = new TextBox();
            _btnBrowseProject = new Button { Text = "浏览..." };
            _btnBrowseMaterial = new Button { Text = "浏览..." };
            _btnRestoreDefaults = new Button { Text = "恢复默认" };
            _btnOk = new Button { Text = "确定" };
            _btnCancel = new Button { Text = "取消" };
            AbortButton = _btnCancel;

            Content = CreateLayout();
            LoadCurrentValues();
            InitializeEvents();
        }

        private void LoadCurrentValues()
        {
            // 显示实际生效的文件全路径，而不是配置里可能为空的原始值
            _txtProjectLibraryPath.Text = _settingsService.ProjectLibraryPath;
            _txtMaterialLibraryPath.Text = _settingsService.MaterialLibraryPath;
        }

        private Control CreateLayout()
        {
            var hint = new Label
            {
                Text = $"配置文件：{FMSettings.SettingsFilePath}\n" +
                       "可以直接填目录（会自动使用默认文件名），也可以填具体的 .json 文件；\n" +
                       "路径支持 %环境变量%；目标文件不存在时会自动创建。",
                TextColor = Colors.Gray,
                Wrap = WrapMode.Word
            };

            var buttons = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 10,
                Items = { _btnRestoreDefaults, new StackLayout { Orientation = Orientation.Horizontal, Spacing = 10, Items = { _btnOk, _btnCancel } } }
            };

            return new TableLayout
            {
                Spacing = new Size(10, 10),
                Padding = new Padding(15),
                Rows =
                {
                    new TableRow(new Label { Text = "项目库文件（Projects.json）：", VerticalAlignment = VerticalAlignment.Center }) { ScaleHeight = false },
                    new TableRow
                    {
                        ScaleHeight = false,
                        Cells =
                        {
                            new TableCell(_txtProjectLibraryPath, true),
                            new TableCell(_btnBrowseProject) { ScaleWidth = false }
                        }
                    },
                    new TableRow(new Label { Text = "物料库文件（MaterialLibrary.json）：", VerticalAlignment = VerticalAlignment.Center }) { ScaleHeight = false },
                    new TableRow
                    {
                        ScaleHeight = false,
                        Cells =
                        {
                            new TableCell(_txtMaterialLibraryPath, true),
                            new TableCell(_btnBrowseMaterial) { ScaleWidth = false }
                        }
                    },
                    new TableRow(new Panel { Height = 1 }) { ScaleHeight = true },
                    new TableRow(hint) { ScaleHeight = false },
                    new TableRow(buttons) { ScaleHeight = false }
                }
            };
        }

        private void InitializeEvents()
        {
            _btnBrowseProject.Click += (s, e) => BrowseFile(
                _txtProjectLibraryPath, "选择项目库文件", FMSettings.ProjectLibraryFileName);

            _btnBrowseMaterial.Click += (s, e) => BrowseFile(
                _txtMaterialLibraryPath, "选择物料库文件", FMSettings.MaterialLibraryFileName);

            _btnRestoreDefaults.Click += (s, e) =>
            {
                _txtProjectLibraryPath.Text = FMSettings.DefaultProjectLibraryPath;
                _txtMaterialLibraryPath.Text = FMSettings.DefaultMaterialLibraryPath;
            };

            _btnOk.Click += (s, e) => ApplySettings();

            _btnCancel.Click += (s, e) => Close();
        }

        private void BrowseFile(TextBox target, string title, string defaultFileName)
        {
            var current = FMSettings.ResolveFilePath(target.Text, defaultFileName)
                          ?? Path.Combine(FMSettings.AppDataDirectory, defaultFileName);

            var dialog = new SaveFileDialog
            {
                Title = title,
                // Eto 的 FileName 是全路径，这里带上当前值，用户从原位置开始找
                FileName = current
            };
            dialog.Filters.Add(new FileFilter("JSON 文件", ".json"));

            if (dialog.ShowDialog(this) != DialogResult.Ok) return;
            if (!string.IsNullOrWhiteSpace(dialog.FileName))
                target.Text = dialog.FileName;
        }

        private void ApplySettings()
        {
            if (string.IsNullOrWhiteSpace(_txtProjectLibraryPath.Text))
            {
                MessageBox.Show(this, "请填写项目库文件路径。", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtMaterialLibraryPath.Text))
            {
                MessageBox.Show(this, "请填写物料库文件路径。", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                return;
            }

            var desired = new FMSettings
            {
                ProjectLibraryPath = _txtProjectLibraryPath.Text,
                MaterialLibraryPath = _txtMaterialLibraryPath.Text
            };

            try
            {
                _settingsService.Apply(desired);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"保存设置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxType.Error);
            }
        }
    }
}
