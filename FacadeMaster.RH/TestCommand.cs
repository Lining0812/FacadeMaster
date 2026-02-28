using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
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
        public override string EnglishName => "FM_Test";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoObject rhobj = null;
            using(GetObject getObj = new GetObject())
            {
                getObj.SetCommandPrompt("选择对象");

                getObj.GeometryFilter = ObjectType.AnyObject;
                getObj.GetMultiple(1, 0);

                foreach (var obj in getObj.Objects())
                {
                    rhobj = obj.Object();
                    if (rhobj != null)
                    {
                        rhobj.Attributes.SetUserString("aaa", "111");
                    }
                }
            }
            return Result.Success;
        }
    }
}
