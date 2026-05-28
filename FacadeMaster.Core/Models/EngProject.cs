using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public string ProjectName { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 项目批次
        /// </summary>
        public ICollection<ProjectBatch> ProjectBatchs { get; set; } = new List<ProjectBatch>();

        /// <summary>
        /// 项目物料
        /// </summary>
        public ICollection<Guid> Materials { get; set; } = new List<Guid>();

        [JsonConstructor]
        private EngProject() { }

        public EngProject(string name, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("工程名称不能为空", nameof(name));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("工程编号不能为空", nameof(code));
            ProjectName = name;
            ProjectCode = code;
        }

        #region 批次管理
        public ProjectBatch AddBatch(string name, string wbs, string processNum)
        {

            if (ProjectBatchs.Any(b => b.WBS == wbs))
                throw new InvalidOperationException($"WBS '{wbs}' 在当前工程下已存在");
            var batch = new ProjectBatch(name, wbs, processNum);
            ProjectBatchs.Add(batch);
            Touch();
            return batch;
        }

        public void RemoveBatch(Guid batchId)
        {
            var batch = GetBatch(batchId);
            if (batch == null) throw new InvalidOperationException($"批次不存在"); ;
            ProjectBatchs.Remove(batch);
            Touch();
        }

        public ProjectBatch GetBatch(Guid batchId)
        {
            return ProjectBatchs.FirstOrDefault(b => b.Id == batchId);
        }
        #endregion
    }
}
