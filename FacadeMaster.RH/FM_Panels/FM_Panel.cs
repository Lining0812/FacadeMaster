using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.Core.Models;
using FacadeMaster.Core.Services;
using Rhino;
using Rhino.DocObjects;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FacadeMaster.RH.FM_Panels
{
    [Guid("D71F0AF8-8106-46EF-A880-3F00F0A0ABB1")]
    public class FM_Panel : Panel
    {
        private Label _label;
        public FM_Panel()
        {
            var button = new Button { Text = "点击我" };

            var gridview = new GridView();
            gridview.Columns.Add(new GridColumn
            {
                HeaderText = "工程名称",
                DataCell = new TextBoxCell(nameof(EngProject.Name)),
                HeaderTextAlignment = TextAlignment.Center,
            });
            gridview.Columns.Add(new GridColumn
            {
                HeaderText = "工程编号",
                DataCell = new TextBoxCell(nameof(EngProject.Num)),
                HeaderTextAlignment = TextAlignment.Center,
            });
            gridview.DataStore = RhinoPlugin.Instance.GetService<IProjectService>().GetAllProjects();

            var layout = new DynamicLayout();
            layout.Spacing = new Size(5, 5);
            layout.AddRow(button);
            layout.AddRow(gridview);

            Content = layout;

            RhinoDoc.SelectObjects += OnSelectionChanged;
            RhinoDoc.DeselectObjects += OnDeselectObjects;
        }

        private void OnSelectionChanged(object sender, RhinoObjectSelectionEventArgs e)
        {
            UpdatePanelContent(e.Document);
        }

        private void OnDeselectObjects(object sender,RhinoObjectSelectionEventArgs e)
        {
            _label.Text = "This is a test panel.";
            RhinoApp.WriteLine("取消选择");
        }

        private void UpdatePanelContent(RhinoDoc doc)
        {
            if (doc == null) return;

            var _select = doc.Objects.GetSelectedObjects(false,false).ToArray();

            if (_select.Length == 1)
            {
                DisplaySingleObjectProperties(_select[0]);
            }
            else
            {
                return;
            }
        }

        private void DisplaySingleObjectProperties(RhinoObject rhObj)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"属性：{rhObj.ObjectType}");
            sb.AppendLine($"aaa:{rhObj.Attributes.LayerIndex}");

            _label.Text = sb.ToString();
        }
    }
}
