using Eto.Drawing;
using Eto.Forms;
using Rhino;
using Rhino.DocObjects;
using Rhino.UI;
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
            _label = new Label
            {
                Text = "This is a test panel.",
                Font = new Eto.Drawing.Font(SystemFont.Default, 20),
                TextColor = Colors.White,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            Content = _label;

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
