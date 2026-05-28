using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH.A99Info
{
    public class DeleteUserString : FM_ButtonComponent
    {
        public DeleteUserString()
            : base("删除用户数据键", "DeleteUserString",
                  "删除对象的用户数据键",
                  "FacadeMaster", "A99信息")
        {
            Message = "删除用户数据";
        }
        public override Guid ComponentGuid => new Guid("2D89E33B-BF80-4A46-B5B3-BF2624B12044");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        protected override string ButtonDisplayName => "删除";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("物体", "物体", "需要获取用户数据键的几何物体", GH_ParamAccess.item);
            pManager.AddTextParameter("用户数据键", "键", "对象的用户数据键列表", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("信息", "信息", "操作是否成功", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!IsButtonActivate())
            {
                DA.SetDataList(0, new List<string>());
                return;
            }

            IGH_GeometricGoo geometryGoo = null;
            List<string> keys = new List<string>();

            if(!DA.GetData(0, ref geometryGoo) || !DA.GetDataList(1, keys)) return;

            Guid refId = geometryGoo.ReferenceID;
            RhinoObject rhinoObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(refId);

            if (rhinoObject == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "未找到对应的Rhino对象");
                DA.SetDataList(0, new List<string>());
                return;
            }

            List<string> res = new List<string>();
            foreach (var key in keys)
            {
                if(rhinoObject.Attributes.DeleteUserString(key)) 
                    res.Add($"键：{key}: 已删除");
                else
                    res.Add($"键：{key}: 删除失败");
            }

            DA.SetDataList(0, res);
        }
    }
}
