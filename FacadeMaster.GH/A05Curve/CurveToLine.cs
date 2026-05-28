using Grasshopper.Kernel;
using Rhino.Geometry;
using System;

namespace FacadeMaster.GH.A05Curve
{
    public class CurveToLine : GH_Component
    {
        public CurveToLine()
            : base("曲线转直线", "CurveToLine",
                  "将输入的曲线转换为直线",
                  "FacadeMaster", "A05曲线")
        {
            Message = "曲线转直线";
        }
        public override Guid ComponentGuid => new Guid("04B7567E-9663-4DE3-87D3-D055CA9B2CB0");
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("曲线", "曲线", "输入的曲线", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("直线", "直线", "输出的直线", GH_ParamAccess.item);
            pManager.AddVectorParameter("方向", "方向", "直线的方向向量", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) return;

            if(curve.IsClosed)
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "输入的曲线是闭合曲线，无法正确转换为直线");

            Line line = new Line(curve.PointAtStart, curve.PointAtEnd);
            Vector3d direction = line.Direction;

            DA.SetData(0, line);
            DA.SetData(1, direction);
        }
    }
}
