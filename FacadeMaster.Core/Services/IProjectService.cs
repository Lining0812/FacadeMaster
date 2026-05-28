using FacadeMaster.Core.Models;
using FacadeMaster.Core.Models.Material;
using System;
using System.Collections.Generic;

namespace FacadeMaster.Core.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// 创建工程项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns>EngProject</returns>
        EngProject CreateProject(string name, string code);

        /// <summary>
        /// 获取项目实例
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        EngProject GetById(Guid projectid);

        /// <summary>
        /// 获取所有项目实例
        /// </summary>
        /// <returns></returns>
        IEnumerable<EngProject> GetAllProjects();

        void DeleteProject(EngProject project);

        void AddBatchToProject(Guid projectId, string name, string wbs, string processNum);

        void RemoveBatchFromProject(Guid projectId, Guid batchId);

        void UpdateBatch(Guid projectId, Guid batchId, string name, string wbs, string processNum);

        /// <summary>
        /// 为项目关联一个全局物料（仅记录引用）
        /// </summary>
        void AddMaterialToProject(Guid projectId, Guid materialId);

        /// <summary>
        /// 移除项目对某物料的引用
        /// </summary>
        void RemoveMaterialFromProject(Guid projectId, Guid materialId);

        /// <summary>
        /// 获取项目实际使用的物料实体（从全局库解析）
        /// </summary>
        IEnumerable<FM_Material> GetProjectMaterials(Guid projectId);
    }
}
