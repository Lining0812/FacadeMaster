using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.GH.A99Info
{
    public class SetUserStrings : FM_ButtonComponent
    {
        public SetUserStrings()
            : base("设置用户数据值", "SetUserStrings",
                  "设置对象的用户数据值",
                  "FacadeMaster", "A99信息")
        {
            Message = "设置用户数据值";
        }
        public override Guid ComponentGuid => new Guid("752C3543-885F-4978-B552-AFBE547A582A");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        protected override string ButtonDisplayName => "写入";

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("物体", "物体", "需要设置用户数据值的几何物体或数据列表", GH_ParamAccess.item);
            pManager.AddTextParameter("键", "键", "需要设置的用户数据键", GH_ParamAccess.item);
            pManager.AddTextParameter("值", "值", "需要设置的用户数据值", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("成功", "成功", "是否成功设置用户数据值", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            throw new NotImplementedException();
        }
    }
}
