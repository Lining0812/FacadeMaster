namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 物料
    /// </summary>
    public class FM_BaseMaterial : BaseEntity
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        public string M_Name { get; set; }
        /// <summary>
        /// 物料属性
        /// </summary>
        public string M_Properties { get; set; }
        /// <summary>
        /// 工程属性编码
        /// </summary>
        public string M_EACode { get; set; }
    }
}
