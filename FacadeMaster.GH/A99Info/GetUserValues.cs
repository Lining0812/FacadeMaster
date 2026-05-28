using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH.A99Info
{
    public class GetUserValues : FM_ButtonComponent
    {
        public GetUserValues()
            : base("获取用户数据值", "GetUserValues",
                  "获取对象的用户数据值列表",
                  "FacadeMaster", "A99信息")
        {
            Message = "获取用户数据值";
        }
        public override Guid ComponentGuid => new Guid("6747A718-1950-4CA2-A458-CE0B92236A38");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        protected override string ButtonDisplayName => "刷新";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("物体", "物体", "需要获取用户数据值的几何物体或数据列表", GH_ParamAccess.item);
            pManager.AddTextParameter("键", "键", "需要获取的用户数据键", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("值", "值", "对象的用户数据值列表", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_GeometricGoo geometryGoo = null;
            List<string> keys = new List<string>();

            if (!DA.GetData(0, ref geometryGoo) || !DA.GetDataList(1, keys)) return;

            Guid refId = geometryGoo.ReferenceID;
            RhinoObject rhinoObject = Rhino.RhinoDoc.ActiveDoc.Objects.Find(refId);

            if (rhinoObject == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "未找到对应的Rhino对象");
                DA.SetDataList(0, new List<string>());
                return;
            }

            var values = new List<string>();
            foreach (var key in keys)
            {
                values.Add(rhinoObject.Attributes.GetUserString(key));
            }

            DA.SetDataList(0, values);
        }
    }
}
