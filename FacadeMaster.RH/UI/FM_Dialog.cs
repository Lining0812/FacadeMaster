using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.Core;
using FacadeMaster.Core.Models;
using FacadeMaster.Core.Models.Material;
using FacadeMaster.RH;
using FacadeMaster.RH.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacadeMaster.UI.Dialogs
{
    public class FM_Dialog : Dialog
    {
        #region UI控件
        private DropDown _projectSelector;
        private Label _lblProjectCode;
        private Label _lblBatchCount;
        private Label _lblMaterialCount;
        private Label _lblCreateAt;
        private Label _lblUpdateAt;
        private GridView _batchGridView;
        private GridView _materialGridView;
        private Button _btnCreateProject;
        private Button _btnDeleteProject;
        private Button _btnAddBatch;
        private Button _btnEditBatch;
        private Button _btnDeleteBatch;
        private Button _btnAddMaterial;
        private Button _btnEditMaterial;
        private Button _btnDeleteMaterial;
        private Button _btnClose;
        #endregion

        private readonly ProjectManager _projectManager;
        private List<ProjectBatch> _currentBatches = new List<ProjectBatch>();
        private List<FM_Material> _currentMaterials = new List<FM_Material>();
        private bool _suppressSelectionEvent;

        public EngProject CurrentProject
        {
            get => _projectManager.ActiveProject;
            set => _projectManager.ActiveProject = value;
        }

        public FM_Dialog(ProjectManager projectManager)
        {
            Title = "工程项目管理";
            MinimumSize = new Size(1000, 700);
            Resizable = true;

            _projectManager = projectManager;

            // 创建UI
            Content = CreateLayout();

            AbortButton = _btnClose;

            InitializeEvents();
            SubscribeManagerEvents();

            // 初次加载
            RefreshAll();
        }

        private void SubscribeManagerEvents()
        {
            _projectManager.ProjectChanged += (s, e) => RefreshAll();
            _projectManager.ActiveProjectChanged += (s, e) =>
            {
                SyncProjectSelector();
                RefreshDataSources();
                UpdateProjectInfo();
            };
            _projectManager.ProjectContentChanged += (s, e) =>
            {
                RefreshDataSources();
                UpdateProjectInfo();
            };
        }

        private Control CreateLayout()
        {
            var projectInfoPanel = CreateProjectInfoPanel();
            var splitPanel = CreateSplitPanel();
            var bottomPanel = CreateBottomPanel();

            return new TableLayout
            {
                Spacing = new Size(10, 10),
                Padding = new Padding(15),
                Rows =
                {
                    new TableRow(projectInfoPanel) { ScaleHeight = false },
                    new TableRow(splitPanel) { ScaleHeight = true },
                    new TableRow(bottomPanel) { ScaleHeight = false }
                }
            };
        }

        // ---------- 顶部：项目信息面板 ----------
        private Control CreateProjectInfoPanel()
        {
            _projectSelector = new DropDown
            {
                Width = 300,
                DataStore = _projectManager.AllProjects,
                ItemTextBinding = Binding.Property<EngProject, string>(p => p.ProjectName)
            };

            _lblProjectCode = new Label { Text = "工程编号: -" };
            _lblBatchCount = new Label { Text = "批次数量: 0" };
            _lblMaterialCount = new Label { Text = "物料数量: 0" };
            _lblCreateAt = new Label { Text = "创建时间: -" };
            _lblUpdateAt = new Label { Text = "更新时间: -" };
            _btnCreateProject = new Button { Text = "创建项目" };
            _btnDeleteProject = new Button { Text = "删除项目" };

            var infoPanel = new TableLayout
            {
                Spacing = new Size(10, 5),
                Rows =
                {
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(new Label { Text = "选择项目:", VerticalAlignment = VerticalAlignment.Center }) { ScaleWidth = false },
                            new TableCell(_projectSelector) { ScaleWidth = false },
                            new TableCell(null) { ScaleWidth = true },
                            new TableCell(_btnCreateProject) { ScaleWidth = false }
                        }
                    },
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(_lblProjectCode) { ScaleWidth = false },
                            new TableCell(_lblBatchCount) { ScaleWidth = false },
                            new TableCell(_lblMaterialCount) { ScaleWidth = false },
                            new TableCell(_btnDeleteProject) { ScaleWidth = false }
                        }
                    },
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(_lblCreateAt) { ScaleWidth = false },
                            new TableCell(_lblUpdateAt) { ScaleWidth = false },
                            new TableCell(null) { ScaleWidth = true }
                        }
                    }
                }
            };

            return new GroupBox
            {
                Text = "项目信息",
                Content = new Panel { Content = infoPanel, Padding = new Padding(10) }
            };
        }

        // ---------- 中间：分割面板 ----------
        private Control CreateSplitPanel()
        {
            var splitter = new Splitter
            {
                Orientation = Orientation.Horizontal,
                Panel1 = CreateBatchPanel(),
                Panel2 = CreateMaterialPanel(),
                SplitterWidth = 5,
            };
            return splitter;
        }

        // ---------- 批次面板 ----------
        private Control CreateBatchPanel()
        {
            _batchGridView = new GridView
            {
                DataStore = _currentBatches,
                ShowHeader = true,
                AllowMultipleSelection = false,
                Border = BorderType.Line,
                Columns =
                {
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<ProjectBatch, string>(p => p.BatchName) },
                        HeaderText = "批次名称", Width = 120
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<ProjectBatch, string>(p => p.WBS) },
                        HeaderText = "WBS", Width = 100
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<ProjectBatch, string>(p => p.ProcessNum) },
                        HeaderText = "工序号", Width = 80
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<ProjectBatch, string>(p => p.CreateAt.ToString()) },
                        HeaderText = "创建时间", Width = 130
                    },
                    new GridColumn
                    {
                        DataCell = new CheckBoxCell { Binding = Binding.Property<ProjectBatch, bool?>(p => p.IsDelete) },
                        HeaderText = "已删除", Width = 60
                    }
                }
            };

            _btnAddBatch = new Button { Text = "新增批次", Width = 80 };
            _btnEditBatch = new Button { Text = "编辑批次", Width = 80 };
            _btnDeleteBatch = new Button { Text = "删除批次", Width = 80 };

            var buttonLayout = new TableLayout
            {
                Spacing = new Size(10, 5),
                Rows =
                {
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(_btnAddBatch) { ScaleWidth = false },
                            new TableCell(_btnEditBatch) { ScaleWidth = false },
                            new TableCell(_btnDeleteBatch) { ScaleWidth = false },
                            new TableCell(null) { ScaleWidth = true }
                        }
                    }
                }
            };

            var layout = new TableLayout
            {
                Spacing = new Size(5, 10),
                Rows =
                {
                    new TableRow(new Label { Text = "项目批次列表", Font = Fonts.Sans(10) }) { ScaleHeight = false },
                    new TableRow(_batchGridView) { ScaleHeight = true },
                    new TableRow(buttonLayout) { ScaleHeight = false }
                }
            };

            return new GroupBox
            {
                Text = "批次管理",
                Content = new Panel { Content = layout, Padding = new Padding(10) }
            };
        }

        // ---------- 物料面板 ----------
        private Control CreateMaterialPanel()
        {
            _materialGridView = new GridView
            {
                DataStore = _currentMaterials,
                ShowHeader = true,
                AllowMultipleSelection = false,
                Border = BorderType.Line,
                Columns =
                {
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<FM_Material, string>(p => p.M_Type.ToString()) },
                        HeaderText = "类型", Width = 60
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<FM_Material, string>(p => p.M_Name) },
                        HeaderText = "物料名称", Width = 110
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<FM_Material, string>(p => p.M_Props) },
                        HeaderText = "物料编码", Width = 100
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<FM_Material, string>(p => p.M_Code) },
                        HeaderText = "工程属性编码", Width = 100
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell { Binding = Binding.Property<FM_Material, string>(p => p.M_Unit) },
                        HeaderText = "单位", Width = 50
                    },
                    new GridColumn
                    {
                        DataCell = new CheckBoxCell { Binding = Binding.Property<FM_Material, bool?>(p => p.IsDelete) },
                        HeaderText = "已删除", Width = 60
                    }
                }
            };

            _btnAddMaterial = new Button { Text = "新增物料", Width = 80 };
            _btnEditMaterial = new Button { Text = "编辑物料", Width = 80 };
            _btnDeleteMaterial = new Button { Text = "删除物料", Width = 80 };

            var buttonLayout = new TableLayout
            {
                Spacing = new Size(10, 5),
                Rows =
                {
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(_btnAddMaterial) { ScaleWidth = false },
                            new TableCell(_btnEditMaterial) { ScaleWidth = false },
                            new TableCell(_btnDeleteMaterial) { ScaleWidth = false },
                            new TableCell(null) { ScaleWidth = true }
                        }
                    }
                }
            };

            var layout = new TableLayout
            {
                Spacing = new Size(5, 10),
                Rows =
                {
                    new TableRow(new Label { Text = "项目物料列表", Font = Fonts.Sans(10) }) { ScaleHeight = false },
                    new TableRow(_materialGridView) { ScaleHeight = true },
                    new TableRow(buttonLayout) { ScaleHeight = false }
                }
            };

            return new GroupBox
            {
                Text = "物料管理",
                Content = new Panel { Content = layout, Padding = new Padding(10) }
            };
        }

        // ---------- 底部面板 ----------
        private Control CreateBottomPanel()
        {
            _btnClose = new Button { Text = "关闭", Width = 100 };

            return new TableLayout
            {
                Spacing = new Size(10, 5),
                Rows =
                {
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(null) { ScaleWidth = true },
                            new TableCell(_btnClose) { ScaleWidth = false }
                        }
                    }
                }
            };
        }

        // ---------- 事件绑定 ----------
        private void InitializeEvents()
        {
            _projectSelector.SelectedIndexChanged += (sender, e) =>
            {
                if (_suppressSelectionEvent) return;
                _projectManager.ActiveProject = _projectSelector.SelectedValue as EngProject;
            };

            _btnClose.Click += (s, e) => Close();

            _btnCreateProject.Click += (s, e) =>
            {
                var dialog = new CreateProjectDialog(_projectManager.Service);
                var res = dialog.ShowModal(this);
                if (res != null)
                {
                    // CreateProjectDialog 内部已入库，这里只刷新 + 切换激活项
                    _projectManager.ReloadProjects();
                    _projectManager.ActiveProject = res;
                }
            };

            _btnDeleteProject.Click += (s, e) =>
            {
                if (CurrentProject == null)
                {
                    MessageBox.Show(this, "还没有任何项目工程", "提示", MessageBoxButtons.OK);
                    return;
                }
                _projectManager.DeleteProject(CurrentProject);
            };

            _btnAddBatch.Click += (s, e) =>
            {
                if (CurrentProject == null)
                {
                    MessageBox.Show(this, "请先选择项目", "提示", MessageBoxButtons.OK);
                    return;
                }
                var dialog = new CreateBatchDialog(_projectManager.Service, CurrentProject.Id);
                dialog.ShowModal(this);
                _projectManager.NotifyProjectContentUpdated();
            };

            _btnEditBatch.Click += (sender, e) =>
            {
                var selected = _batchGridView.SelectedItem as ProjectBatch;
                if (selected == null)
                    MessageBox.Show(this, "请先选择一个批次", "提示", MessageBoxButtons.OK);
                else
                    MessageBox.Show(this, $"编辑批次: {selected.BatchName}", "提示", MessageBoxButtons.OK);
            };

            _btnDeleteBatch.Click += (sender, e) =>
            {
                var selected = _batchGridView.SelectedItem as ProjectBatch;
                if (selected == null)
                    MessageBox.Show(this, "请先选择一个批次", "提示", MessageBoxButtons.OK);
                else
                    MessageBox.Show(this, $"删除批次: {selected.BatchName}", "提示", MessageBoxButtons.OK);
            };

            _btnAddMaterial.Click += (sender, e) =>
                MessageBox.Show(this, "新增物料功能开发中...", "提示", MessageBoxButtons.OK);

            _btnEditMaterial.Click += (sender, e) =>
            {
                var selected = _materialGridView.SelectedItem as FM_Material;
                if (selected == null)
                    MessageBox.Show(this, "请先选择一个物料", "提示", MessageBoxButtons.OK);
                else
                    MessageBox.Show(this, $"编辑物料: {selected.M_Name}", "提示", MessageBoxButtons.OK);
            };

            _btnDeleteMaterial.Click += (sender, e) =>
            {
                var selected = _materialGridView.SelectedItem as FM_Material;
                if (selected == null)
                    MessageBox.Show(this, "请先选择一个物料", "提示", MessageBoxButtons.OK);
                else
                    MessageBox.Show(this, $"删除物料: {selected.M_Name}", "提示", MessageBoxButtons.OK);
            };
        }

        /// <summary>整体刷新：项目列表 + 选择器 + 数据源 + 信息面板</summary>
        private void RefreshAll()
        {
            _projectSelector.DataStore = _projectManager.AllProjects;
            SyncProjectSelector();
            RefreshDataSources();
            UpdateProjectInfo();
        }

        private void RefreshDataSources()
        {
            _currentBatches = _projectManager.GetBatches(CurrentProject);
            _currentMaterials = _projectManager.GetMaterials(CurrentProject);

            _batchGridView.DataStore = _currentBatches;
            _materialGridView.DataStore = _currentMaterials;
        }

        private void SyncProjectSelector()
        {
            _suppressSelectionEvent = true;
            _projectSelector.SelectedValue = CurrentProject;
            _suppressSelectionEvent = false;
        }

        private void UpdateProjectInfo()
        {
            var p = CurrentProject;
            if (p == null)
            {
                _lblProjectCode.Text = "工程编号: -";
                _lblBatchCount.Text = "批次数量: 0";
                _lblMaterialCount.Text = "物料数量: 0";
                _lblCreateAt.Text = "创建时间: -";
                _lblUpdateAt.Text = "更新时间: -";
                return;
            }
            _lblProjectCode.Text = $"工程编号: {p.ProjectCode}";
            _lblBatchCount.Text = $"批次数量: {_currentBatches.Count}";
            _lblMaterialCount.Text = $"物料数量: {_currentMaterials.Count}";
            _lblCreateAt.Text = $"创建时间: {p.CreateAt:yyyy-MM-dd HH:mm}";
            _lblUpdateAt.Text = $"更新时间: {p.UpdateAt:yyyy-MM-dd HH:mm}";
        }
    }
}