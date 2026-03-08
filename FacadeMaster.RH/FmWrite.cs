using FacadeMaster.RH.FM_UserData;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using System;

namespace FacadeMaster.RH
{
    public class FmWrite : Command
    {
        public FmWrite()
        {
            Instance = this;
        }
        public static FmWrite Instance { get; private set; }
        public override string EnglishName => "FM_Write";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            ObjRef objref;
            var rc = RhinoGet.GetOneObject("select one", false, ObjectType.AnyObject, out objref);
            if (rc != Result.Success) return rc;

            var rhobj = objref.Object();
            if (rhobj == null) return Result.Failure;

            var ud = rhobj.Attributes.UserData.Find(typeof(JGJData)) as JGJData;
            if(ud == null)
            {
                ud = new JGJData();
                rhobj.Attributes.UserData.Add(ud);
                rhobj.CommitChanges();
            }
            else
            {
                RhinoApp.WriteLine("{0} = {1}", ud.Description, ud);
            }
            return Result.Success;
        }
    }
}
