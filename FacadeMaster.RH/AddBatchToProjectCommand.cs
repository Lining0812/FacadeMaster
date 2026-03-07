using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using System;

namespace FacadeMaster.RH
{
    public class AddBatchToProjectCommand : Command
    {
        public AddBatchToProjectCommand()
        {
            Instance = this;
        }
        public static AddBatchToProjectCommand Instance { get; private set; }
        public override string EnglishName => "FM_AddBatch";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            string id = "";
            using (GetString getStr = new GetString())
            {
                getStr.SetCommandPrompt("输入工程编号");
                getStr.Get();
                id = getStr.StringResult();
            }

            var service = new ProjectService(new ProjectRepository());
            var dto = new CreateBatchRequest("z", "z", "z");

            Guid.TryParse(id, out Guid gid);
            service.AddBatchToProject(gid, dto);

            RhinoApp.WriteLine("增加批次成功");
            return Result.Success;
        }
    }
}
