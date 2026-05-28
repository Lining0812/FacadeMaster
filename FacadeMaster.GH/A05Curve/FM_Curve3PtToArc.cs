using Grasshopper.Kernel;
using Rhino.Geometry;
using System;

namespace FacadeMaster.GH.A05Curve
{
    public class FM_Curve3PtToArc : GH_Component
    {
        public FM_Curve3PtToArc()
            : base("曲线转圆弧", "3PtToArc",
                  "输入的曲线通过三点转换为圆弧",
                  "FacadeMaster", "A05曲线")
        {
            Message = "曲线三点转圆弧";
        }
        public override Guid ComponentGuid => new Guid("A6B52156-17D6-4971-9FBF-43B0F0A24BAB");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("曲线", "曲线", "输入的曲线", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddArcParameter("圆弧", "圆弧", "输出的圆弧", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) return;

            if (curve.IsClosed)
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的曲线是闭合曲线，无法正确转换为圆弧");

            Arc arc = new Arc(curve.PointAtStart, curve.PointAtNormalizedLength(0.5), curve.PointAtEnd);

            DA.SetData(0, arc);
        }
    }
}
