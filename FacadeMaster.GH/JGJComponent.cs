using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Linq;

namespace FacadeMaster.GH
{
    public class JGJComponent : GH_Component
    {
        public JGJComponent()
            : base("JGJComponent", "JGJ",
            "JGJ Data",
            "FacadeMaster", "Primitive")
        {
        }
        public override Guid ComponentGuid => new Guid("E738B85C-B553-42B2-A8DE-958C15736BE6");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("brep", "b", "brep", GH_ParamAccess.item);
            pManager.AddPlaneParameter("plane", "p", "plane", GH_ParamAccess.item, Plane.WorldXY);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("data", "d", "JGJData", GH_ParamAccess.item);
            pManager.AddBooleanParameter("success", "s", "是否成功", GH_ParamAccess.item);
            //pManager.AddTextParameter("BH", "bh", "编号", GH_ParamAccess.item);
            //pManager.AddPlaneParameter("plane", "p", "plane", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Brep brep = null;
            GH_Plane plane = null;
            if (!DA.GetData(0, ref brep) || !DA.GetData(1,ref plane)) return;

            var rhobj = RhinoDoc.ActiveDoc.Objects.Find(brep.ReferenceID);
            if (rhobj != null)
            {
                //rhobj.Attributes.UserDictionary.Set("Test_BH", "GH测试编号");
                //rhobj.Attributes.UserDictionary.Set("Test_Plane", plane.Value);

                // 可以查询到FacadeMaster.RH.FM_UserData.JGJData，但是没有引用.GH项目，所以无法直接使用JGJData类
                var suc = rhobj.Attributes.UserData.Contains(new Guid("AED87E30-EA39-48DA-9242-59886E3F2FDD"));
                var res = rhobj.Attributes.UserData[0];

                DA.SetData(0, res);
                DA.SetData(1, suc);
                //DA.SetData(3, "GH测试编号");
                //DA.SetData(1, plane.Value);
            }
        }
    }
}
