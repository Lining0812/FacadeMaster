using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;

namespace FacadeMaster.RH
{
    public class TestCommand : Command
    {
        public TestCommand()
        {
            Instance = this;
        }
        public static TestCommand Instance { get; private set; }
        public override string EnglishName => "FM_TestCommand";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("--- 开始添加项目批次（直接回车结束）---");
            while (true)
            {
                using(GetString getstr = new GetString())
                {
                    getstr.SetCommandPrompt("输入文本");
                    getstr.Get();
                    var str = getstr.StringResult();
                    RhinoApp.WriteLine(str);
                }

                using (GetString getstr = new GetString())
                {
                    getstr.SetCommandPrompt("是否继续 Y/N");
                    getstr.Get();
                    var Continue = getstr.StringResult();
                    if(Continue.ToUpper() == "N")
                    {
                        RhinoApp.WriteLine("结束");
                        return Result.Success;
                    }
                }
            }
        }
    }
}
