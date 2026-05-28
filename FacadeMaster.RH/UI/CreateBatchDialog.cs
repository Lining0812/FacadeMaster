using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.Core.Services;
using System;

namespace FacadeMaster.RH.UI
{
    public class CreateBatchDialog : Dialog
    {
        #region UI控件
        private Label _lblBatchName;
        private Label _lblBatchWbs;
        private Label _lblBatchNum;
        private TextBox _txtBatchName;
        private TextBox _txtBatchWbs;
        private TextBox _txtBatchNum;
        private Button _btnConfirm;
        private Button _btnCancel;
        #endregion

        IProjectService _projectService;
        private Guid _projectId;

        public CreateBatchDialog(IProjectService projectService,Guid projectId)
        {
            Title = "创建项目批次";
            Resizable = false;

            _projectService = projectService;
            _projectId = projectId;

            Content = CreateLayout();

            AbortButton = _btnCancel;
            DefaultButton = _btnConfirm;
        }

        private Control CreateLayout()
        {
            _lblBatchName = new Label { Text = "批次名称" ,Width=80 };
            _lblBatchWbs = new Label { Text = "WBS元素号", Width = 80 };
            _lblBatchNum = new Label { Text = "工序号", Width = 80 };

            _txtBatchName = new TextBox { Width = 250 };
            _txtBatchWbs = new TextBox { Width = 250 };
            _txtBatchNum = new TextBox { Width = 250 };

            _btnConfirm = new Button { Text = "确定" };
            _btnCancel = new Button { Text = "取消" };

            // 确定按钮点击事件
            _btnConfirm.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(_txtBatchName.Text))
                {
                    MessageBox.Show(this, "请输入批次名称！", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(_txtBatchWbs.Text))
                {
                    MessageBox.Show(this, "请输入WBS元素号！", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(_txtBatchNum.Text))
                {
                    MessageBox.Show(this, "请输入工序号！", "提示", MessageBoxButtons.OK, MessageBoxType.Warning);
                    return;
                }

                try
                {
                    _projectService.AddBatchToProject(_projectId, _txtBatchName.Text, _txtBatchWbs.Text, _txtBatchNum.Text);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"创建项目失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxType.Error);
                }
            };

            // 取消按钮点击事件
            _btnCancel.Click += (sender, e) =>
            {
                Close();
            };

            var layout = new TableLayout
            {
                Spacing = new Size(10, 10),
                Padding = new Padding(20),
                Rows =
                {
                    new TableRow{Cells={new TableCell(_lblBatchName),new TableCell(_txtBatchName) }},
                    new TableRow{Cells={new TableCell(_lblBatchWbs),new TableCell(_txtBatchWbs) }},
                    new TableRow{Cells={new TableCell(_lblBatchNum),new TableCell(_txtBatchNum) }},
                    new TableRow{ScaleHeight=true,Cells={new TableCell(),new TableCell()}},
                    new TableRow{Cells={new TableCell(new StackLayout { Orientation=Orientation.Horizontal,
                    Spacing=10,Items={new StackLayout(),_btnConfirm,_btnCancel}})}}
                }
            };
            return layout;
        }
    }
}
