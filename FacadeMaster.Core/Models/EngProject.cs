using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.Core.Models
{
    /// <summary>
    /// 工程项目（engineering project）
    /// </summary>
    public class EngProject
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
        public ICollection<ProjectBatch> ProjectBatchs { get; set; }
    }
}
