using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.GH
{
    public class TestComponent : GH_Component

    {
        public TestComponent()
            : base("TestComponent", "Test",
                  "A test component for FacadeMaster",
                  "FacadeMaster", "Test")
        {
        }
        public override Guid ComponentGuid => new Guid("F81E8A2F-35D9-4C1C-80E6-00C97054B652");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "Input Brep", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "Output Plane", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            Plane plane = Plane.Unset;

            if (!DA.GetData(0, ref brep)) return;

            var lin = brep.Faces.OrderByDescending(f => AreaMassProperties.Compute(f).Area).First();
            if(lin.SetDomain(0, new Interval(0, 1)) && lin.SetDomain(1,new Interval(0,1)))
            {
                lin.FrameAt(0.5, 0.5, out plane);
            }

            DA.SetData(0, plane);
        }
    }
}
