using FacadeMaster.Shared;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.RH
{
    public class FmRead : Command
    {
        public override string EnglishName => "FM_Read";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoObject rhobj = null;
            using (GetObject getObj = new GetObject())
            {
                getObj.SetCommandPrompt("选择对象");

                getObj.GeometryFilter = ObjectType.AnyObject;
                getObj.GetMultiple(1, 0);

                foreach (var obj in getObj.Objects())
                {
                    rhobj = obj.Object();
                    if (rhobj != null)
                    {
                        CustomUserData data = (CustomUserData)rhobj.Attributes.UserData.Find(typeof(CustomUserData));
                        RhinoApp.WriteLine($"自定义数据：{data.Test}");
                        RhinoApp.WriteLine($"变换次数：{data.Count}");
                    }
                }
            }
            return Result.Success;
        }
    }
}
