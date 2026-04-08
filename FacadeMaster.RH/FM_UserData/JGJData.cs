using FacadeMaster.Core.Enums;
using FacadeMaster.Core.Models.Material;
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
    public abstract class JGJData<TMaterial> : UserData where TMaterial : FM_BaseMaterial
    {
        public string BH { get; set; }
        public string JGTH { get; set; }
        public Plane BH_Plane { get; set; } = Plane.Unset;
        public Plane CT_Plane { get; set; } = Plane.Unset;
        public abstract TMaterial Material { get; set; }

        public override string Description => "加工件";

        public override string ToString()
        {
            return string.Format("编号平面={0},出图平面={1}", BH_Plane, CT_Plane);
        }

        protected override void OnDuplicate(UserData source)
        {
            if (source is JGJData<TMaterial> data)
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
            Plane bhPlane = BH_Plane;
            Plane ctPlane = CT_Plane;
            if (!bhPlane.Transform(transform) || !ctPlane.Transform(transform)) return;
            
            BH_Plane = bhPlane;
            CT_Plane = ctPlane;
            base.OnTransform(transform);
        }

        public override bool ShouldWrite => true;

        protected override bool Read(BinaryArchiveReader archive)
        {
            var dic = archive.ReadDictionary();

            if (!GetMaterial(dic) || !dic.ContainsKey("编号") || !dic.ContainsKey("加工图号") 
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
                || !SetMaterial(dic)
                ) return false;

            archive.WriteDictionary(dic);
            return true;
        }

        protected virtual bool SetMaterial(ArchivableDictionary dic)
        {
            if (Material == null || Material.M_Type == MaterialType.None) return false;

            dic.Set("物料类型", (int)Material.M_Type);
            dic.Set("物料名称", Material.M_Name);
            dic.Set("物料属性", Material.M_Props);
            dic.Set("工程属性编码", Material.M_Code);
            dic.Set("单位", Material.M_Unit);

            return true;
        }

        protected virtual bool GetMaterial(ArchivableDictionary dic)
        {
            int m_type;
            if (!dic.TryGetInteger("物料类型", out m_type) 
                || (MaterialType)m_type == MaterialType.None) return false;
            
            Material.M_Name = dic.GetString("物料名称");
            Material.M_Props = dic.GetString("物料属性");
            Material.M_Code = dic.GetString("工程属性编码");
            Material.M_Unit = dic.GetString("单位");
            
            return true;
        }
    }
}
