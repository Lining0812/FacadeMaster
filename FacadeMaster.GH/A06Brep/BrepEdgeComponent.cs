using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH.A06Brep
{
    public class BrepEdgeComponent : GH_Component
    {
        public BrepEdgeComponent()
            : base("曲面边线分解", "BrepEdge",
                  "将曲面的边线区分为外边线、内边线和孔边线",
                  "FacadeMaster", "A06Brep")
        {
            Message = "分解多重曲面边线";
        }
        public override Guid ComponentGuid => new Guid("7CEEB9EB-AB82-4591-966F-1A816BF707DE");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("brep", "b", "brep", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("edges", "外边", "曲面外边线", GH_ParamAccess.list);
            pManager.AddCurveParameter("edges", "内边", "曲面内边线", GH_ParamAccess.list);
            pManager.AddCurveParameter("edges", "孔边", "曲面孔边线", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            if(!DA.GetData(0, ref brep)) return;

            List<Curve> outCurves = new List<Curve>();
            List<Curve> innerCurves = new List<Curve>();
            List<Curve> holeCurves = new List<Curve>();
            HashSet<int> edgeIndexSet = new HashSet<int>();

            foreach (BrepLoop eLoop in brep.Loops)
            {
                if (eLoop.LoopType == BrepLoopType.Outer)
                {
                    foreach (BrepTrim trim in eLoop.Trims)
                    {
                        BrepEdge edge = trim.Edge;
                        int edgeIndex = edge.EdgeIndex;
                        if (!edgeIndexSet.Add(edgeIndex)) continue;

                        if (edge.TrimCount == 1) 
                            outCurves.Add(edge.DuplicateCurve());
                        else 
                            innerCurves.Add(edge.DuplicateCurve());
                    }
                }
                else if (eLoop.LoopType == BrepLoopType.Inner)
                {
                    foreach (BrepTrim trim in eLoop.Trims)
                    {
                        BrepEdge edge = trim.Edge;
                        holeCurves.Add(edge.DuplicateCurve());
                    }
                }
            }

            DA.SetDataList(0, outCurves);
            DA.SetDataList(1, innerCurves);
            DA.SetDataList(2, holeCurves);
        }
    }
}
