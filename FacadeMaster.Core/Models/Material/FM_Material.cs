using FacadeMaster.Core.Enums;

namespace FacadeMaster.Core.Models.Material
{
    /// <summary>
    /// 物料属性
    /// </summary>
    public abstract class FM_Material : BaseEntity
    {
        /// <summary>
        /// 物料类型
        /// </summary>
        public MaterialType M_Type { get; protected set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string M_Name { get; set; } = string.Empty;
        /// <summary>
        /// 物料编码
        /// </summary>
        public string M_Props { get; set; } = string.Empty;
        /// <summary>
        /// 工程属性编码
        /// </summary>
        public string M_Code { get; set; } = string.Empty;
        /// <summary>
        /// 单位
        /// </summary>
        public string M_Unit { get; set; } = "块";

        public override bool IsValid => base.IsValid && Validate();

        protected FM_Material(MaterialType type)
        {
            M_Type = type;
        }

        /// <summary>
        /// 有效性验证
        /// </summary>
        /// <returns>如果值有效返回Ture</returns>
        public virtual bool Validate()
        {
            if (string.IsNullOrWhiteSpace(M_Name)) return false;
            if (string.IsNullOrWhiteSpace(M_Props)) return false;
            if (string.IsNullOrWhiteSpace(M_Code)) return false;
            return true;
        }
    }
}   
