using FacadeMaster.RH.FM_UserData;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;

namespace FacadeMaster.RH.FM_Command
{
    public class FmWriteCommand : Command
    {
        public FmWriteCommand()
        {
            Instance = this;
        }
        public static FmWriteCommand Instance { get; private set; }
        public override string EnglishName => "FM_Write";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            ObjRef objref;
            var rc = RhinoGet.GetOneObject("select one object", false, ObjectType.AnyObject, out objref);
            if (rc != Result.Success) return rc;

            var rhobj = objref.Object();
            if (rhobj == null) return Result.Failure;

            var ud = rhobj.Attributes.UserData.Find(typeof(JGJData)) as JGJData;
            if(ud == null)
            {
                ud = new JGJData("Test_BH", "Test_JGTH", Plane.WorldXY, Plane.WorldXY);
                rhobj.Attributes.UserData.Add(ud);
                rhobj.CommitChanges();
            }
            else
            {
                RhinoApp.WriteLine("编号：{0}, 加工图号：{1}", ud.BH, ud.JGTH);
            }
            return Result.Success;
        }
    }
}
