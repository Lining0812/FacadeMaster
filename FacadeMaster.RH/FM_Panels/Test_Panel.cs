using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.RH.FM_UserData;
using Rhino;
using Rhino.DocObjects;
using System;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_Panels
{
    [Guid("72FDE7BC-8B8A-4CD2-8856-DE184B23B4FD")]
    public class Test_Panel : Panel
    {
        private readonly TextBox _bhTextBox;
        private readonly TextBox _jgthTextBox;
        private readonly TextBox _bhPlaneTextBox;
        private readonly TextBox _ctPlaneTextBox;

        public Test_Panel()
        {
            _bhTextBox = new TextBox();
            _jgthTextBox = new TextBox();
            _bhPlaneTextBox = new TextBox();
            _ctPlaneTextBox = new TextBox();

            BuildLayout();

            RhinoDoc.SelectObjects += OnSelectionChanged;
            RhinoDoc.DeselectObjects += OnDeselectObjects;
            RhinoDoc.DeselectAllObjects += OnDeselectAllObjects;
        }

        private void BuildLayout()
        {
            var mainLayout = new DynamicLayout
            {
                DefaultSpacing = new Size(0, 1),
                Padding = new Padding(0)
            };

            var layout1 = CreateTest1();
            var layout2 = CreateTest2();

            mainLayout.Add(layout1);
            mainLayout.Add(layout2);
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
                    new TableRow(new TableCell(new Label { Text = "编号" }, true), new TableCell(_bhTextBox, true)),
                    new TableRow(new TableCell(new Label { Text = "加工图号" }, true), new TableCell(_jgthTextBox, true)),
                    new TableRow(new TableCell(new Label { Text = "编号平面" }, true), new TableCell(_bhPlaneTextBox, true)),
                    new TableRow(new TableCell(new Label { Text = "出图平面" }, true), new TableCell(_ctPlaneTextBox, true)),
                }
            };

            group.Content = table;

            return group;
        }

        private Control CreateTest2()
        {
            var layout = new DynamicLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            layout.AddRow(new Label { Text = "Test1" }, new ColorPicker { Value = Colors.Red });
            layout.AddRow(new Label { Text = "Test2" }, new CheckBox());
            layout.Add(null);

            return layout;
        }

        private void OnSelectionChanged(object sender, RhinoObjectSelectionEventArgs e)
        {
            // 使用 e.RhinoObjects 获取本次选中的物体，避免 GetSelectedObjects 的时序问题
            var rhinoObjs = e.RhinoObjects;
            RhinoApp.WriteLine("[Test_Panel] SelectObjects triggered, count={0}", rhinoObjs?.Length ?? 0);

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

        private void OnDeselectObjects(object sender, RhinoObjectSelectionEventArgs e)
        {
            ClearTextBoxes();
        }

        private void OnDeselectAllObjects(object sender, RhinoDeselectAllObjectsEventArgs e)
        {
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            _bhTextBox.Text = string.Empty;
            _jgthTextBox.Text = string.Empty;
            _bhPlaneTextBox.Text = string.Empty;
            _ctPlaneTextBox.Text = string.Empty;
        }
    }
}
