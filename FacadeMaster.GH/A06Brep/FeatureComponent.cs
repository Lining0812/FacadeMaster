using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacadeMaster.GH.A06Brep
{
    public class FeatureComponent : GH_Component
    {
        public FeatureComponent()
            : base("特征计算", "Feature",
                  "获取几何体的计算特征",
                  "FacadeMaster", "A06Brep")
        {
        }
        public override Guid ComponentGuid => new Guid("26D71C9E-4070-4086-B30F-88D86E1BCD0A");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("几何体", "几何体", "几何体", GH_ParamAccess.item);
            pManager.AddIntegerParameter("序号", "序号", "序号", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("特征曲面", "特征曲面", "特征曲面", GH_ParamAccess.item);
            pManager.AddPlaneParameter("特征平面", "特征平面", "特征平面", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            if(!DA.GetData(0,ref brep)) return;

            var featureSrf = GetMaximumSurface(brep);
            var featurePlane = GetFeaturePlane(featureSrf);
            DA.SetData(0, featureSrf);
            DA.SetData(1, featurePlane);
        }

        private Surface GetMaximumSurface(Brep brep)
        {
            var faces = new List<Surface>();
            for(int i=0;i<brep.Faces.Count;i++)
            {
                var face = brep.Faces[i];
                faces.Add(face.DuplicateSurface());
            }
            return faces.OrderByDescending(s => AreaMassProperties.Compute(s).Area).First();
        }

        private Plane GetFeaturePlane(Surface surface)
        {
            Plane plane = new Plane();
            if (surface.SetDomain(0, new Interval(0, 1)) && surface.SetDomain(1, new Interval(0, 1)))
            {
                surface.FrameAt(0.5, 0.5, out plane);
                return plane;
            }
            else return Plane.Unset;
        }

    }
}
