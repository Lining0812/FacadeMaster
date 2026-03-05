using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;
using System.Collections.Generic;

namespace FacadeMaster.RH
{
    /// <summary>
    /// 新增工程项目
    /// </summary>
    public class CreateProjectCommand : Command
    {
        public CreateProjectCommand()
        {
            Instance = this;
        }
        public static CreateProjectCommand Instance { get; private set; }
        public override string EnglishName => "FM_CreateProject";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            string p_name = "";
            string p_num = "";
            List<CreateBatchRequest> batches = new List<CreateBatchRequest>();


            using (GetString getStr = new GetString())
            {
                getStr.SetCommandPrompt("输入工程名称");
                getStr.Get();
                p_name = getStr.StringResult();
            }

            using (GetString getStr = new GetString())
            {
                getStr.SetCommandPrompt("输入工程编号");
                getStr.Get();
                p_num = getStr.StringResult();
            }

            while (true)
            {
                using (GetString getStr = new GetString())
                {
                    getStr.SetCommandPrompt("输入批次名称，回车结束");
                    var res = getStr.Get();
                    if (res == GetResult.String)
                    {
                        var batchName = getStr.StringResult();
                        batches.Add(new CreateBatchRequest(batchName, $"WBS_{batchName}", $"Process_{batchName}"));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            CreateProjectRequest request = new CreateProjectRequest(p_name, p_num);

            var service = new ProjectService(new ProjectRepository());
            var project = service.CreateProject(request);

            RhinoApp.WriteLine($"Name:{project.Name},Number:{project.Num}");
            RhinoApp.WriteLine($"CreateAt:{project.CreateAt}");

            return Result.Success;
        }
    }
}
