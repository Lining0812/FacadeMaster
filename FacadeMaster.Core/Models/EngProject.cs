using System;
using System.Collections.Generic;

namespace FacadeMaster.Core.Models
{
    /// <summary>
    /// 工程项目（engineering project）
    /// </summary>
    public class EngProject : BaseEntity
    {
        /// <summary>
        /// 工程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// 项目批次
        /// </summary>
        public ICollection<ProjectBatch> ProjectBatchs { get; set; } = new List<ProjectBatch>();
    }
}
