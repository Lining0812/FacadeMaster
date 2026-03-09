using FacadeMaster.RH.FM_UserData;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.GH
{
    public class JGJComponent : GH_Component
    {
        public JGJComponent()
            : base("JGJComponent", "JGJ",
            "JGJ Data",
            "FacadeMater", "Primitive")
        {
        }
        public override Guid ComponentGuid => new Guid("E738B85C-B553-42B2-A8DE-958C15736BE6");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("brep", "b", "brep", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("plane", "p", "plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("plane", "p", "plane", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Brep brep = null;
            if (!DA.GetData(0, ref brep)) return;

            var rhobj = RhinoDoc.ActiveDoc.Objects.Find(brep.ReferenceID);
            if (rhobj != null)
            {
                var data = rhobj.Attributes.UserData.Find(typeof(JGJData)) as JGJData;
                DA.SetData(0, data.BH_Plane);
                DA.SetData(1, data.CT_Plane);
            }
        }
    }
}
