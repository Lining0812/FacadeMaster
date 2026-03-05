namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 铝板物料
    /// </summary>
    public class FM_AluPlate : FM_BaseMaterial
    {
        /// <summary>
        /// 铝板厚度
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// 铝板色号
        /// </summary>
        public string Color { get; set; }
    }
}
