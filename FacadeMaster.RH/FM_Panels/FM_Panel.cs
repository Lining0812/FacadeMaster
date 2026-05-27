using Eto.Drawing;
using Eto.Forms;
using FacadeMaster.RH.FM_UserData;
using Rhino;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_Panels
{
    [Guid("D71F0AF8-8106-46EF-A880-3F00F0A0ABB1")]
    public class FM_Panel : Panel
    {
        private readonly GridView _gridview;

        public FM_Panel()
        {
            _gridview = new GridView();
            _gridview.Columns.Add(new GridColumn
            {
                HeaderText = "编号",
                DataCell = new TextBoxCell(nameof(JGJRow.BH)),
                HeaderTextAlignment = TextAlignment.Center,
            });
            _gridview.Columns.Add(new GridColumn
            {
                HeaderText = "加工图号",
                DataCell = new TextBoxCell(nameof(JGJRow.JGTH)),
                HeaderTextAlignment = TextAlignment.Center,
            });

            var layout = new DynamicLayout();
            layout.Spacing = new Size(5, 5);
            layout.AddRow(_gridview);

            Content = layout;

            RhinoDoc.SelectObjects += OnSelectionChanged;
            RhinoDoc.DeselectObjects += OnDeselectObjects;
            RhinoDoc.DeselectAllObjects += onDeselectAllObjects;
        }

        private void onDeselectAllObjects(object sender, RhinoDeselectAllObjectsEventArgs e)
        {
            RhinoApp.WriteLine("All对象已取消选择，刷新面板内容");
            _gridview.DataStore = null;
        }

        private void OnSelectionChanged(object sender, RhinoObjectSelectionEventArgs e)
        {
            UpdatePanelContent(e.Document);
        }

        private void OnDeselectObjects(object sender, RhinoObjectSelectionEventArgs e)
        {
            RhinoApp.WriteLine("对象已取消选择，刷新面板内容");
            _gridview.DataStore = null;
        }

        private void UpdatePanelContent(RhinoDoc doc)
        {
            if (doc == null) return;

            var rows = doc.Objects.GetSelectedObjects(false, false)
                .Select(rhObj => rhObj.Attributes.UserData.Find(typeof(JGJData)) as JGJData)
                .Where(ud => ud != null)
                .Select(ud => new JGJRow { BH = ud.BH, JGTH = ud.JGTH })
                .ToList<object>();

            _gridview.DataStore = rows.Count > 0 ? rows : null;
        }
    }

    public class JGJRow
    {
        public string BH { get; set; }
        public string JGTH { get; set; }
    }
}
