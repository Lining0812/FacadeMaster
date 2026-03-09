using FacadeMaster.Core.Models.Material;
using Rhino.Collections;
using Rhino.DocObjects.Custom;
using Rhino.FileIO;
using Rhino.Geometry;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_UserData
{
    [Guid("AED87E30-EA39-48DA-9242-59886E3F2FDD")]
    public abstract class JGJData : UserData
    {
        public string BH { get; set; }
        public string JGTH { get; set; }
        public Plane BH_Plane { get; set; } = Plane.WorldXY;
        public Plane CT_Plane { get; set; } = Plane.WorldXY;
        public FM_BaseMaterial Material { get; set; }

        public override string Description => "JGJData";

        public override string ToString()
        {
            return string.Format("编号平面={0},出图平面={1}", BH_Plane, CT_Plane);
        }

        protected override void OnDuplicate(UserData source)
        {
            JGJData data = source as JGJData;
            if (data != null)
            {
                BH = data.BH;
                JGTH = data.JGTH;
                BH_Plane = data.BH_Plane;
                CT_Plane = data.CT_Plane;
                Material = data.Material;
            }
            base.OnDuplicate(source);
        }

        protected override void OnTransform(Transform transform)
        {
            // 平面跟随操作变换
            Plane bhPlane = BH_Plane;
            Plane ctPlane = CT_Plane;
            if (bhPlane.Transform(transform) && ctPlane.Transform(transform))
            {
                BH_Plane = bhPlane;
                CT_Plane = ctPlane;
            }
            base.OnTransform(transform);
        }

        public override bool ShouldWrite => true;

        protected override bool Read(BinaryArchiveReader archive)
        {
            var dic = archive.ReadDictionary();
            if (dic.ContainsKey("编号平面") && dic.ContainsKey("出图平面"))
            {
                BH_Plane = dic.GetPlane("编号平面");
                CT_Plane = dic.GetPlane("出图平面");
            }
            return true;
        }

        protected override bool Write(BinaryArchiveWriter archive)
        {
            var dic = new ArchivableDictionary();
            dic.Set("BH", BH);
            dic.Set("JGTH", JGTH);
            dic.Set("编号平面", BH_Plane);
            dic.Set("出图平面", CT_Plane);
            archive.WriteDictionary(dic);

            return true;
        }

        protected virtual void SetMaterial(ArchivableDictionary dic)
        {
            if (Material != null)
            {
                dic.Set("M_Type", (int)Material.M_Type);
                dic.Set("M_Name", Material.M_Name);
                dic.Set("M_Props", Material.M_Props);
                dic.Set("M_Code", Material.M_Code);
                dic.Set("M_Unit", Material.M_Unit);
            }
        }

        protected virtual void GetMaterial(ArchivableDictionary dic)
        {
            //if (dic.ContainsKey("M_Type") && dic.ContainsKey("M_Name") && dic.ContainsKey("M_Props") && dic.ContainsKey("M_Code") && dic.ContainsKey("M_Unit"))
            //{
            //    Material = new FM_BaseMaterial
            //    {
            //        M_Type = (Core.Enums.MaterialType)dic.Getint("M_Type", 0),
            //        M_Name = dic.GetString("M_Name"),
            //        M_Props = dic.GetString("M_Props"),
            //        M_Code = dic.GetString("M_Code"),
            //        M_Unit = dic.GetString("M_Unit")
            //    };
            //}
        }
    }
}
