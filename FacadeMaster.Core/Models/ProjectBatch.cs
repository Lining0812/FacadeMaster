using Newtonsoft.Json;
using System;
using System.Xml.Linq;

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
        public string BatchName { get; set; }
        /// <summary>
        /// WBS元素号
        /// </summary>
        public string WBS { get; set; }
        /// <summary>
        /// 工序号
        /// </summary>
        public string ProcessNum { get; set; }

        [JsonConstructor]
        private ProjectBatch() { }

        public ProjectBatch(string name, string wbs, string num)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("批次名称不能为空", nameof(name));
            if (string.IsNullOrWhiteSpace(wbs))
                throw new ArgumentException("WBS元素号不能为空", nameof(wbs));
            if (string.IsNullOrWhiteSpace(num))
                throw new ArgumentException("工序号不能为空", nameof(num));
            BatchName = name;
            WBS = wbs;
            ProcessNum = num;
        }

        public void UpdateInfo(string name, string wbs, string processNum)
        {
            if (!string.IsNullOrWhiteSpace(name)) BatchName = name;
            if (!string.IsNullOrWhiteSpace(wbs)) WBS = wbs;
            if (!string.IsNullOrWhiteSpace(processNum)) ProcessNum = processNum;
            Touch();
        }
    }
}
