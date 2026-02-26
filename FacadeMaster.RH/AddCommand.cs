using FacadeMaster.Core.Models;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;

namespace FacadeMaster.RH
{
    /// <summary>
    /// 新增工程项目
    /// </summary>
    public class AddCommand : Command
    {
        public AddCommand()
        {
            Instance = this;
        }
        public static AddCommand Instance { get; private set; }
        public override string EnglishName => "FM_AddProject";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            string p_name = "";
            string p_num = "";


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

            var service = new ProjectService(new ProjectRepository());
            service.CreateProject(p_name, p_num);

            RhinoApp.WriteLine($"Name:{p_name},Number:{p_num}");
            //RhinoApp.WriteLine($"CreateAt:{project.CreateAt}");

            return Result.Success;
        }
    }
}
