using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.Core.Models;
using FacadeMaster.Core.Services;
using System;

namespace FacadeMaster.RH.UI
{
    public class CreateProjectDialog : Dialog<EngProject>
    {
        #region UI控件
        private Label _lblProjectName;
        private Label _lblProjectCode;
        private TextBox _txtProjectName;
        private TextBox _txtProjectCode;
        private Button _btnConfirm;
        private Button _btnCancel;
        #endregion

        IProjectService _projectService;

        public CreateProjectDialog(IProjectService projectService)
        {
            Title = "创建工程项目";
            Resizable = false;

            _projectService = projectService;

            Content = CreateLayout();
        }

        private Control CreateLayout()
        {
            _lblProjectName = new Label { Text = "工程名称" ,Width=80 };
            _lblProjectCode = new Label { Text = "工程编号", Width = 80 };

            _txtProjectName = new TextBox { Width = 250 };
            _txtProjectCode = new TextBox { Width = 250 };

            _btnConfirm = new Button { Text = "确定" };
            _btnCancel = new Button { Text = "取消" };

            // 确定按钮点击事件
            _btnConfirm.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(_txtProjectName.Text))
                {
                    MessageBox.Show(this, "请输入工程名称！", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(_txtProjectCode.Text))
                {
                    MessageBox.Show(this, "请输入工程编号！", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                    return;
                }

                try
                {
                    var project = _projectService.CreateProject(_txtProjectName.Text, _txtProjectCode.Text);
                    Result = project;
                    Close(Result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"创建项目失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            };

            // 取消按钮点击事件
            _btnCancel.Click += (sender, e) =>
            {
                Result = null;
                Close(Result);
            };

            var layout = new TableLayout
            {
                Spacing = new Size(10, 10),
                Padding = new Padding(20),
                Rows =
                {
                    new TableRow{Cells={new TableCell(_lblProjectName),new TableCell(_txtProjectName)}},
                    new TableRow{Cells={new TableCell(_lblProjectCode),new TableCell(_txtProjectCode)}},
                    new TableRow{ScaleHeight=true,Cells={new TableCell(),new TableCell()}},
                    new TableRow{Cells={new TableCell(new StackLayout { Orientation=Orientation.Horizontal,
                    Spacing=10,Items={new StackLayout(),_btnConfirm,_btnCancel}})}}
                }
            };
            return layout;
        }
    }
}
