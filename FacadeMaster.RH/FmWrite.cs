using FacadeMaster.Shared;
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

            var ud = rhobj.Attributes.UserData.Find(typeof(CustomUserData)) as CustomUserData;
            if(ud == null)
            {
                ud = new CustomUserData();
                rhobj.Attributes.UserData.Add(ud);
                rhobj.CommitChanges();
            }
            else
            {
                RhinoApp.WriteLine("{0} = {1}", ud.Description, ud);
            }
            return Result.Success;
        }

        //protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        //{
        //    using (GetObject getObj = new GetObject())
        //    {
        //        getObj.SetCommandPrompt("选择对象");

        //        getObj.GeometryFilter = ObjectType.AnyObject;
        //        getObj.GetMultiple(1, 0);

        //        foreach (var obj in getObj.Objects())
        //        {
        //            var rhobj = obj.Object();
        //            CustomUserData data = new CustomUserData();

        //            if (rhobj == null) continue;
        //            RhinoApp.WriteLine($"对象 ID: {rhobj.Id}, 类型: {rhobj.ObjectType}");

        //            try
        //            {
        //                rhobj.Attributes.UserData.Add(data);
        //                RhinoApp.WriteLine("添加成功");
        //            }
        //            catch(Exception ex)
        //            {
        //                RhinoApp.WriteLine($"添加失败: {ex.Message}");
        //            }
        //        }
        //    }
        //    return Result.Success;
        //}
    }
}
