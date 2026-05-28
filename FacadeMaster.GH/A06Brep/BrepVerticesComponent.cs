using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH.A06Brep
{
    public class BrepVerticesComponent : GH_Component
    {
        public BrepVerticesComponent()
            : base("曲面顶点", "BrepVertices",
                  "将多重曲面分解为顶点",
                  "FacadeMaster", "A06Brep")
        {
            Message = "曲面分解顶点";
        }

        public override Guid ComponentGuid => new Guid("3963E71F-4747-4CED-8C19-6A9344F33E3B");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("brep", "b", "brep", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("vertices", "v", "vertices", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            if (!DA.GetData(0, ref brep)) return;

            List<Point3d> vertices = new List<Point3d>();
            foreach (BrepVertex vertex in brep.Vertices)
            {
                vertices.Add(vertex.Location);
            }

            DA.SetDataList(0, vertices);
        }
    }
}
