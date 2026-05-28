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
    [Guid("AED87E30-EA39-48DA-9242-59886E3F2FDD")]
    public class JGJData : UserData
    {        
        public string BH { get; set; }
        public string JGTH { get; set; }
        public Plane BH_Plane { get; set; } = Plane.Unset;
        public Plane CT_Plane { get; set; } = Plane.Unset;
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(BH)
                    && !string.IsNullOrWhiteSpace(JGTH)
                    && BH_Plane.IsValid 
                    && CT_Plane.IsValid;
            }
        }

        /// <summary>
        /// 必须有Public的无参构造
        /// </summary>
        public JGJData()
        { 
        }

        public JGJData(string bh,string jgth,Plane bh_plane, Plane ct_plane)
        {
            BH = bh;
            JGTH = jgth;
            BH_Plane = bh_plane;
            CT_Plane = ct_plane;
        }

        public override string Description => "FM加工件属性";

        public override bool ShouldWrite => true;

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
        }

        protected override void OnTransform(Transform transform)
        {
            Plane bhPlane = BH_Plane;
            Plane ctPlane = CT_Plane;
            if (!bhPlane.Transform(transform) || !ctPlane.Transform(transform)) return;
            
            BH_Plane = bhPlane;
            CT_Plane = ctPlane;
            base.OnTransform(transform);
            //RhinoApp.WriteLine("用户数据已变换，编号平面={0},出图平面={1}", BH_Plane, CT_Plane);
        }

        protected override bool Read(BinaryArchiveReader archive)
        {
            bool rc = false;
            var dic = archive.ReadDictionary();

            if (!dic.ContainsKey("编号") || !dic.ContainsKey("加工图号") 
                || !dic.ContainsKey("编号平面") || !dic.ContainsKey("出图平面"))
            return false;
            
            BH = dic.GetString("编号");
            JGTH = dic.GetString("加工图号");
            BH_Plane = dic.GetPlane("编号平面");
            CT_Plane = dic.GetPlane("出图平面");

            return !archive.ReadErrorOccured;
        }

        protected override bool Write(BinaryArchiveWriter archive)
        {
            bool rc = false;
            var dic = new ArchivableDictionary();

            if(!dic.Set("编号", BH) || !dic.Set("加工图号", JGTH)
                || !dic.Set("编号平面", BH_Plane) || !dic.Set("出图平面", CT_Plane) 
                ) return false;

            archive.WriteDictionary(dic);
            return !archive.WriteErrorOccured;
        }
    }
}
