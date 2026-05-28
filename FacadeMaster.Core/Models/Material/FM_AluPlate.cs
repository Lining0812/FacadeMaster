using FacadeMaster.Core.Enums;

namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 铝板物料属性
    /// </summary>
    public class FM_AluPlate : FM_Material
    {
        /// <summary>
        /// 铝板厚度
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// 铝板色号
        /// </summary>
        public string Color { get; set; }

        public FM_AluPlate() : base(MaterialType.AluminumPlate)
        {
            Thickness = 0.0;
            Color = "详见封样";
        }
        public FM_AluPlate(string name, string props, string code, double thickness, string color) : base(MaterialType.AluminumPlate)
        {
            M_Name = name;
            M_Props = props;
            M_Code = code;
            M_Unit = "块";
            Thickness = thickness;
            Color = color;
        }

        public override bool Validate()
        {
            if(!base.Validate() || Thickness <= 0.0) return false;
            return true;
        }

        public override string ToString() => $"[{M_Type}] {M_Name} | 物料编码：{M_Code}";
    }
}
