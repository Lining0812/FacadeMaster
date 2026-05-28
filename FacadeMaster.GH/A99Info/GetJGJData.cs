using FacadeMaster.RH.FM_UserData;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;

namespace FacadeMaster.GH.A99Info
{
    public class GetJGJData : FM_ButtonComponent
    {
        public GetJGJData()
            :base("获取加工件数据", "GetJGJData",
                  "获取对象的加工件数据",
                  "FacadeMaster", "A99信息")
        {
            Message = "获取加工件数据";
        }

        public override Guid ComponentGuid => new Guid("76336D90-7EF5-4D78-B0BF-7741F83CA0BD");

        protected override string ButtonDisplayName => "刷新";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("物体", "物体", "需要获取用户数据值的几何物体或数据列表", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("编号", "编号", "对象的编号数据", GH_ParamAccess.item);
            pManager.AddTextParameter("图号", "图号", "对象的图号数据", GH_ParamAccess.item);
            pManager.AddPlaneParameter("编号平面", "编号平面", "编号平面", GH_ParamAccess.item);
            pManager.AddPlaneParameter("出图平面", "出图平面", "出图平面", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_GeometricGoo goo = null;
            if (!DA.GetData(0, ref goo)) return;

            var rhobj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(goo.ReferenceID);

            if(rhobj == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "未找到对象，请检查输入的几何体是否在Rhino文档中。");
                return;
            }

            var jgjData = rhobj.Attributes.UserData.Find(typeof(JGJData)) as JGJData;

            if(jgjData == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "对象未附加JGJ数据，请检查对象是否包含JGJ数据。");
                return;
            }

            DA.SetData(0, jgjData.BH);
            DA.SetData(1, jgjData.JGTH);
            DA.SetData(2, jgjData.BH_Plane);
            DA.SetData(3, jgjData.CT_Plane);
        }
    }
}
