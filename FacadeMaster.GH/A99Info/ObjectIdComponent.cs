using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH.A99Info
{
    public class ObjectIdComponent : GH_Component
    {
        public ObjectIdComponent()
            : base("外协编号", "ObjectId",
                  "加工件外协编号生成，批次号需为两位字母或数字。（物料-批次-流水号）",
                  "FacadeMaster", "A99信息")
        {
            Message = "生成外协编号";
        }
        
        public override Guid ComponentGuid => new Guid("F0C27971-C2A3-4324-8046-30EBDC45D62F");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("物体", "物体", "需要编号的几何体", GH_ParamAccess.list);
            pManager.AddTextParameter("物料", "物料", "物料", GH_ParamAccess.item, "LB");
            pManager.AddTextParameter("批次", "批次号", "批次号", GH_ParamAccess.item, "01");
            pManager.AddTextParameter("流水字母", "流水字母", "流水字母", GH_ParamAccess.item, "A");
            pManager.AddIntegerParameter("起始序号", "起始序号", "起始序号", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("编号", "编号", "编号", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> objs = new List<IGH_Goo>();
            string material = "LB";
            string batch = string.Empty;
            string serialLetter = "A";
            int startNumber = 0;

            if (!DA.GetDataList(0, objs) || !DA.GetData(1,ref material) 
                || !DA.GetData(2, ref batch) || !DA.GetData(3,ref serialLetter) || !DA.GetData(4, ref startNumber)
               ) return;

            if (startNumber < 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "起始序号不能为负数"); return;
            }

            serialLetter = serialLetter.ToUpper();

            List<string> numbers = new List<string>(objs.Count);
            for(int i =0;i< objs.Count; i++)
            {
                int serial = startNumber + i;

                // 当超过99时，流水字母自动进位（A->B, B->C, ...）
                int letterOffset = serial / 100;
                int numberPart = serial % 100;

                char currentLetter = (char)(serialLetter[0] + letterOffset);
                // 检查是否超出Z范围
                if (currentLetter > 'Z')
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
                        $"流水号超出范围，第 {i + 1} 个物体使用了 '{currentLetter}'，超过字母Z");
                }

                string serialStr = numberPart.ToString("D2");
                string fullNumber = $"{material}-{batch}-{currentLetter}{serialStr}";
                numbers.Add(fullNumber);
            }

            DA.SetDataList(0, numbers);
        }
    }
}
