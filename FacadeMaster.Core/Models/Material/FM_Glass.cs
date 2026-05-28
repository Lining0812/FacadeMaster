using FacadeMaster.Core.Enums;
using System.Drawing;

namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 玻璃物料
    /// </summary>
    public class FM_Glass : FM_Material
    {
        /// <summary>
        /// 玻璃厚度
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// 玻璃膜系
        /// </summary>
        public string Glass_Film { get; set; }

        public FM_Glass() : base(MaterialType.Glass)
        {
            Thickness = 0.0;
            Glass_Film = "详见封样";
        }
    }
}
