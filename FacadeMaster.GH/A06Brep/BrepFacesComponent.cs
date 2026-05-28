using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH.A06Brep
{
    public class BrepFacesComponent : GH_Component
    {
        public BrepFacesComponent()
            : base("曲面子面", "BrepFaces",
                  "将多重曲面分解为单一曲面",
                  "FacadeMaster", "A06Brep")
        {
            Message = "曲面分解子面";
        }
        public override Guid ComponentGuid => new Guid("9F72EDC8-6A6D-4578-9D7E-37AA90DF94D1");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("brep", "b", "brep", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("surfaces", "f", "surfaces", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            if (!DA.GetData(0, ref brep)) return;

            List<Surface> surfaces = new List<Surface>();
            foreach (BrepFace face in brep.Faces)
            {
                surfaces.Add(face.DuplicateSurface());
            }

            DA.SetDataList(0, surfaces);
        }
    }
}
