using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FacadeMaster.GH.A99Info
{
    public class GetUserKeys : FM_ButtonComponent
    {
        public GetUserKeys()
            : base("获取用户数据键", "GetUserKeys",
                  "获取对象的用户数据键列表",
                  "FacadeMaster", "A99信息")
        {
            Message = "获取用户数据键";
        }
        public override Guid ComponentGuid => new Guid("507D923C-83DB-43DD-8817-44FBB20525FE");
        protected override Bitmap Icon => Properties.Resource.Test;
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        protected override string ButtonDisplayName => "刷新";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("物体", "物体", "需要获取用户数据键的几何物体", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("用户数据键", "键", "对象的用户数据键列表", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_GeometricGoo geometryGoo = null;

            if(!DA.GetData(0, ref geometryGoo)) return;

            Guid refId = geometryGoo.ReferenceID;
            RhinoObject rhinoObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(refId);

            if (rhinoObject == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "未找到对应的Rhino对象");
                DA.SetDataList(0, new List<string>());
                return;
            }

            var keys = rhinoObject.Attributes.GetUserStrings().AllKeys;

            DA.SetDataList(0, keys);
        }
    }
}
