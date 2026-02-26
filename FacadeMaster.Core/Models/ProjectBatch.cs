namespace FacadeMaster.Core.Models
{
    /// <summary>
    /// 项目批次
    /// </summary>
    public class ProjectBatch : BaseEntity
    {
        /// <summary>
        /// 批次名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// WBS元素号
        /// </summary>
        public string WBS { get; set; }
        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessNum { get; set; }
    }
}
