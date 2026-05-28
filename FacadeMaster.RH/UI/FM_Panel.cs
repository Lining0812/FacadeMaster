using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.Core;
using FacadeMaster.Core.Models;
using FacadeMaster.Core.Settings;
using FacadeMaster.RH.FM_UserData;
using FacadeMaster.UI.Dialogs;
using Rhino;
using Rhino.DocObjects;
using Rhino.UI;
using System;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_Panels
{
    [Guid("72FDE7BC-8B8A-4CD2-8856-DE184B23B4FD")]
    public class FM_Panel : Panel
    {
        private readonly ProjectManager _projectManager;

        private readonly TextBox _bhTextBox;
        private readonly TextBox _jgthTextBox;
        private readonly TextBox _bhPlaneTextBox;
        private readonly TextBox _ctPlaneTextBox;
        private readonly Button _projectButton;
        private readonly Button _settingsButton;
        private readonly DropDown _projectDropDown;
        private readonly GridView _batchGridView;
        private bool _suppressSelectionEvent;

        public FM_Panel()
        {
            _projectManager = RhinoPlugin.Instance.ProjectManager;
            RhinoApp.WriteLine($"[FM_Panel] Loaded {_projectManager.AllProjects.Count} projects from the database.");

            _bhTextBox = new TextBox();
            _jgthTextBox = new TextBox();
            _bhPlaneTextBox = new TextBox();
            _ctPlaneTextBox = new TextBox();
            _projectButton = new Button()
            {
                Text = "项目管理",
            };
            _projectButton.Click += (s, e) =>
            {
                var parent = RhinoEtoApp.MainWindowForDocument(RhinoDoc.ActiveDoc);
                var dialog = new FM_Dialog(_projectManager);
                dialog.ShowModal(parent);
            };
            _settingsButton = new Button()
            {
                Text = "数据位置",
            };
            _settingsButton.Click += (s, e) => ShowSettingsDialog();
            _projectDropDown = new DropDown
            {
                DataStore = _projectManager.AllProjects,
                ItemTextBinding = Binding.Property<EngProject, string>(p => p.ProjectName)
            };

            _batchGridView = new GridView
            {
                Height = 500,
                ShowHeader = true,
                AllowMultipleSelection = false,
                Columns =
                {
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<ProjectBatch, string>(b => b.BatchName),
                            TextAlignment = TextAlignment.Left
                        },
                        HeaderText = "批次名称",
                        HeaderTextAlignment = TextAlignment.Center,
                        Width = 120
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<ProjectBatch, string>(b => b.WBS),
                            TextAlignment = TextAlignment.Center
                        },
                        HeaderText = "WBS元素号",
                        HeaderTextAlignment = TextAlignment.Center
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<ProjectBatch, string>(b => b.ProcessNum),
                            TextAlignment = TextAlignment.Center
                        },
                        HeaderText = "工序号",
                        HeaderTextAlignment = TextAlignment.Center
                    },
                }
            };

            BuildLayout();
            SubscribeManagerEvents();

            // 初次同步
            SyncFromManager();

            RhinoDoc.SelectObjects += OnSelectionChanged;
            RhinoDoc.DeselectObjects += OnDeselectObjects;
            RhinoDoc.DeselectAllObjects += OnDeselectAllObjects;
        }

        /// <summary>打开“本地数据位置”设置（与 FMSettings 命令是同一个对话框）</summary>
        private void ShowSettingsDialog()
        {
            var settingsService = RhinoPlugin.Instance?.GetService<FMSettingsService>();
            if (settingsService == null)
            {
                RhinoApp.WriteLine("FacadeMaster 插件服务未初始化，无法打开设置。");
                return;
            }

            var parent = RhinoEtoApp.MainWindowForDocument(RhinoDoc.ActiveDoc);
            var dialog = new FMSettingsDialog(settingsService);
            dialog.ShowModal(parent);

            // 换库后项目列表可能整体变化，ProjectManager 已触发事件，这里再兜底刷新一次
            _projectDropDown.DataStore = _projectManager.AllProjects;
            RefreshBatchGrid();
        }

        private void SubscribeManagerEvents()
        {
            _projectManager.ProjectChanged += (s, e) => SyncFromManager();
            _projectManager.ActiveProjectChanged += (s, e) => SyncFromManager();
            _projectManager.ProjectContentChanged += (s, e) => RefreshBatchGrid();
        }

        /// <summary>把 ProjectManager 的状态同步到 UI</summary>
        private void SyncFromManager()
        {
            _suppressSelectionEvent = true;
            _projectDropDown.DataStore = _projectManager.AllProjects;
            _projectDropDown.SelectedValue = _projectManager.ActiveProject;
            _suppressSelectionEvent = false;

            RefreshBatchGrid();
        }

        private void RefreshBatchGrid()
        {
            var batches = _projectManager.GetBatches(_projectManager.ActiveProject);
            _batchGridView.DataStore = batches;
        }

        private void BuildLayout()
        {
            var mainLayout = new DynamicLayout
            {
                DefaultSpacing = new Size(0, 1),
                Padding = new Padding(5)
            };

            var layout1 = CreateTest1();
            var layout2 = CreateTest2();

            mainLayout.Add(layout1);
            mainLayout.Add(layout2);
            mainLayout.Add(new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 5,
                Items = { _projectButton, _settingsButton }
            });
            this.Content = mainLayout;
        }

        private Control CreateTest1()
        {
            var group = new GroupBox
            {
                Text = "加工件信息",
                Padding = new Padding(5)
            };

            var table = new TableLayout
            {
                Spacing = new Size(5, 5),
                Padding = new Padding(5),
                Rows =
                {
                    new TableRow(new TableCell(new Label { Text = "编号：" }, true), new TableCell(_bhTextBox, true)),
                    new TableRow(new TableCell(new Panel{Height=1,BackgroundColor=Colors.Gray})),
                    new TableRow(new TableCell(new Label { Text = "加工图号：" }, true), new TableCell(_jgthTextBox, true)),
                    new TableRow(new TableCell(new Panel{Height=1,BackgroundColor=Colors.Gray})),
                    new TableRow(new TableCell(new Label { Text = "编号平面：" }, true), new TableCell(_bhPlaneTextBox, true)),
                    new TableRow(new TableCell(new Panel{Height=1,BackgroundColor=Colors.Gray})),
                    new TableRow(new TableCell(new Label { Text = "出图平面：" }, true), new TableCell(_ctPlaneTextBox, true))
                }
            };

            group.Content = table;

            return group;
        }

        private Control CreateTest2()
        {
            var group = new GroupBox { Text = "项目信息", Padding = new Padding(5) };

            _projectDropDown.SelectedValueChanged += (s, e) =>
            {
                if (_suppressSelectionEvent) return;
                var selected = _projectDropDown.SelectedValue as EngProject;
                _projectManager.ActiveProject = selected;
                // ActiveProjectChanged 会触发 SyncFromManager -> RefreshBatchGrid
            };

            var layout = new DynamicLayout
            {
                DefaultSpacing = new Size(0, 5),
                Padding = new Padding(5)
            };
            layout.Add(new Label { Text = "当前项目：" });
            layout.Add(_projectDropDown);
            layout.Add(_batchGridView);

            group.Content = layout;
            return group;
        }

        private void OnSelectionChanged(object sender, RhinoObjectSelectionEventArgs e)
        {
            var rhinoObjs = e.RhinoObjects;

            if (rhinoObjs != null && rhinoObjs.Length == 1)
            {
                var jgjData = rhinoObjs[0].Attributes.UserData.Find(typeof(JGJData)) as JGJData;
                if (jgjData != null)
                {
                    _bhTextBox.Text = jgjData.BH ?? string.Empty;
                    _jgthTextBox.Text = jgjData.JGTH ?? string.Empty;
                    _bhPlaneTextBox.Text = jgjData.BH_Plane.ToString();
                    _ctPlaneTextBox.Text = jgjData.CT_Plane.ToString();
                }
            }
            else if (rhinoObjs != null && rhinoObjs.Length > 1)
            {
                _bhTextBox.Text = "混合";
                _jgthTextBox.Text = "混合";
                _bhPlaneTextBox.Text = "混合";
                _ctPlaneTextBox.Text = "混合";
            }
        }

        private void OnDeselectObjects(object sender, RhinoObjectSelectionEventArgs e) => ClearTextBoxes();

        private void OnDeselectAllObjects(object sender, RhinoDeselectAllObjectsEventArgs e) => ClearTextBoxes();

        private void ClearTextBoxes()
        {
            _bhTextBox.Text = string.Empty;
            _jgthTextBox.Text = string.Empty;
            _bhPlaneTextBox.Text = string.Empty;
            _ctPlaneTextBox.Text = string.Empty;
        }

    }
}
