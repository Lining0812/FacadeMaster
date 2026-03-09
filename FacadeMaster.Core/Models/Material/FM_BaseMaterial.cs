using FacadeMaster.Core.Enums;

namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 物料
    /// </summary>
    public abstract class FM_BaseMaterial : BaseEntity
    {
       /// <summary>
       /// 物料类型
       /// </summary>
        public MaterialType M_Type { get; set; } = MaterialType.None;
        /// <summary>
        /// 物料名称
        /// </summary>
        public string M_Name { get; set; } = "物料名称";
        /// <summary>
        /// 物料属性
        /// </summary>
        public string M_Props { get; set; } = "物料属性";
        /// <summary>
        /// 工程属性编码
        /// </summary>
        public string M_Code { get; set; } = "工程属性编码";
        /// <summary>
        /// 单位
        /// </summary>
        public string M_Unit { get; set; } = "块";
    }
}
