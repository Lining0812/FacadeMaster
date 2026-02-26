using FacadeMaster.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.Core.Models
{
    /// <summary>
    /// 图框
    /// </summary>
    public class DrawingFrame
    {
        /// <summary>
        /// 图纸名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图纸编号
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// 图框大小
        /// </summary>
        public PaperSize Size { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        public int Ratio { get; set; }

        /// <summary>
        /// 版号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 设计阶段
        /// </summary>
        public string DesignPhase { get; set; }
    }
}
