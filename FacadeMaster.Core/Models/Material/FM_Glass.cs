using FacadeMaster.Core.Enums;

namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 玻璃物料
    /// </summary>
    public class FM_Glass : FM_BaseMaterial
    {
        public override MaterialType M_Type => MaterialType.Glass;

        /// <summary>
        /// 玻璃厚度
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// 玻璃膜系
        /// </summary>
        public string Glass_Film { get; set; }
    }
}
