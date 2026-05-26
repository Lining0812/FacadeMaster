using FacadeMaster.Core.Enums;
using FacadeMaster.Core.Models.Material;
using Rhino;
using Rhino.Collections;
using Rhino.DocObjects.Custom;
using Rhino.FileIO;
using Rhino.Geometry;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_UserData
{
    /// <summary>
    /// 加工件基类
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    [Guid("AED87E30-EA39-48DA-9242-59886E3F2FDD")]
    public class JGJData : UserData
    {        public string BH { get; set; }
        public string JGTH { get; set; }
        public Plane BH_Plane { get; set; } = Plane.Unset;
        public Plane CT_Plane { get; set; } = Plane.Unset;

        public override string Description => "FM加工件属性";

        public override string ToString()
        {
            return Description;
        }

        protected override void OnDuplicate(UserData source)
        {
            if (source is JGJData data)
            {
                BH = data.BH;
                JGTH = data.JGTH;
                BH_Plane = data.BH_Plane;
                CT_Plane = data.CT_Plane;
            }
            //base.OnDuplicate(source);
            RhinoApp.WriteLine("已复制，编号平面={0},出图平面={1}", BH_Plane, CT_Plane);
        }

        protected override void OnTransform(Transform transform)
        {
            Plane bhPlane = BH_Plane;
            Plane ctPlane = CT_Plane;
            if (!bhPlane.Transform(transform) || !ctPlane.Transform(transform)) return;
            
            BH_Plane = bhPlane;
            CT_Plane = ctPlane;
            base.OnTransform(transform);
            RhinoApp.WriteLine("用户数据已变换，编号平面={0},出图平面={1}", BH_Plane, CT_Plane);
        }

        public override bool ShouldWrite => true;

        protected override bool Read(BinaryArchiveReader archive)
        {
            var dic = archive.ReadDictionary();

            if (!dic.ContainsKey("编号") || !dic.ContainsKey("加工图号") 
                || !dic.ContainsKey("编号平面") || !dic.ContainsKey("出图平面"))
            return false;
            
            BH = dic.GetString("编号");
            JGTH = dic.GetString("加工图号");
            BH_Plane = dic.GetPlane("编号平面");
            CT_Plane = dic.GetPlane("出图平面");

            return true;
        }

        protected override bool Write(BinaryArchiveWriter archive)
        {
            var dic = new ArchivableDictionary();

            if(!dic.Set("编号", BH) || !dic.Set("加工图号", JGTH)
                || !dic.Set("编号平面", BH_Plane) || !dic.Set("出图平面", CT_Plane) 
                ) return false;

            archive.WriteDictionary(dic);
            return true;
        }
    }
}
