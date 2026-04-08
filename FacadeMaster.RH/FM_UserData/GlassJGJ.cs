using FacadeMaster.Core.Models.Material;
using Rhino.Collections;

namespace FacadeMaster.RH.FM_UserData
{
    /// <summary>
    /// 玻璃加工件
    /// </summary>
    public class GlassJGJ : JGJData<FM_Glass>
    {
        public override string Description => "玻璃加工件";

        public override FM_Glass Material { get; set; } = new FM_Glass();

        protected override bool SetMaterial(ArchivableDictionary dic)
        {
            if(!base.SetMaterial(dic) || !dic.Set("厚度", Material.Thickness) 
                || !dic.Set("玻璃膜系", Material.Glass_Film)) return false;

            return true;
        }

        protected override bool GetMaterial(ArchivableDictionary dic)
        {
            if (!base.GetMaterial(dic) || !dic.ContainsKey("厚度") 
                || !dic.ContainsKey("玻璃膜系")) return false;
            
            Material.Thickness = dic.GetDouble("厚度");
            Material.Glass_Film = dic.GetString("玻璃膜系");

            return true;
        }
    }
}
