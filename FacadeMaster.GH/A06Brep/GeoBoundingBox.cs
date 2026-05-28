using Grasshopper.Kernel;
using Rhino.Geometry;
using System;


namespace FacadeMaster.GH.A06Brep
{
    public class GeoBoundingBox : GH_Component
    {
        public override Guid ComponentGuid => new Guid("B5FC38A2-F7BA-4582-AEA1-5E7C57A336BE");
        public GeoBoundingBox()
            :base("几何体包围盒", "BoundingBox",
                  "几何体包围盒",
                  "FacadeMaster", "A06Brep")
        {
            Message = "包围盒";
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("几何体", "几何体", "几何体", GH_ParamAccess.item);
            pManager.AddPlaneParameter("平面", "平面", "平面", GH_ParamAccess.item, Plane.WorldXY);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("包围盒", "包围盒", "包围盒", GH_ParamAccess.item);
            pManager.AddRectangleParameter("矩形框", "矩形框", "矩形框", GH_ParamAccess.item);
            pManager.AddNumberParameter("宽度", "宽度", "宽度", GH_ParamAccess.item);
            pManager.AddNumberParameter("长度", "长度", "长度", GH_ParamAccess.item);
            pManager.AddNumberParameter("高度", "高度", "高度", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase goo = null;
            Plane plane = Plane.Unset;

            if (!DA.GetData(0, ref goo) || !DA.GetData(1, ref plane)) return;

            Box localBox;
            goo.GetBoundingBox(plane, out localBox);

            var rec = new Rectangle3d(plane, localBox.X, localBox.Y);

            var w = localBox.X.Length;
            var h = localBox.Y.Length;
            var z = localBox.Z.Length;

            DA.SetData(0, localBox);
            DA.SetData(1, rec);
            DA.SetData(2, w);
            DA.SetData(3, h);
            DA.SetData(4, z);
        }
    }
}
