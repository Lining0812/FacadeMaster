using FacadeMaster.RH.FM_UserData;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using System;

namespace FacadeMaster.GH.A99Info
{
    public class CreateJGJData : FM_ButtonComponent
    {
        public CreateJGJData()
            : base("创建加工件", "CreateJGJData",
            "创建加工件数据(必须是Rhino文档中已存在的对象)",
            "FacadeMaster", "A99信息")
        {
            Message = "创建加工件";
        }
        public override Guid ComponentGuid => new Guid("E738B85C-B553-42B2-A8DE-958C15736BE6");
        protected override string ButtonDisplayName => "写入";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("物体", "物体", "物体", GH_ParamAccess.item);
            pManager.AddTextParameter("编号", "编号", "编号", GH_ParamAccess.item);
            pManager.AddTextParameter("图号", "图号", "图号", GH_ParamAccess.item);
            pManager.AddPlaneParameter("编号平面", "编号平面", "编号平面", GH_ParamAccess.item);
            pManager.AddPlaneParameter("出图平面", "出图平面", "出图平面", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("加工件数据", "数据", "加工件数据", GH_ParamAccess.item);
            pManager.AddBooleanParameter("信息", "信息", "创建是否成功", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsButtonActivate())
            {
                DA.SetData(0, null);
                DA.SetData(1, false);
                return;
            }

            GH_Brep brep = null;
            string bh = string.Empty;
            string jgth = string.Empty;
            GH_Plane bhPlane = null;
            GH_Plane jgthPlane = null;

            if (!DA.GetData(0, ref brep) || !DA.GetData(1, ref bh) || !DA.GetData(2, ref jgth) ||
                !DA.GetData(3,ref bhPlane) || !DA.GetData(4, ref jgthPlane)) return;

            var rhobj = RhinoDoc.ActiveDoc.Objects.Find(brep.ReferenceID);
            if (rhobj != null)
            {
                var jgjData = new JGJData(bh, jgth, bhPlane.Value, jgthPlane.Value);
                var suc = rhobj.Attributes.UserData.Add(jgjData);

                DA.SetData(0, jgjData);
                DA.SetData(1, suc);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "未找到对象，请检查输入的几何体是否在Rhino文档中。");
                DA.SetData(0, null);
                DA.SetData(1, false);
            }
        }
    }
}
