using FacadeMaster.Core.DTOs;
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
        /// <param name="dto"></param>
        /// <returns>EngProject</returns>
        EngProject CreateProject(CreateProjectRequest dto);

        /// <summary>
        /// 添加批次至工程项目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        ProjectBatch AddBatchToProject(Guid projectid, CreateBatchRequest dto);

        /// <summary>
        /// 获取所有项目批次
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        ICollection<ProjectBatch> GetBatchesByProject(EngProject p);

        /// <summary>
        /// 添加物料至工程项目
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="material"></param>
        void AddMaterialToProject(Guid projectid, FM_BaseMaterial material);

        /// <summary>
        /// 获取项目实例
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        EngProject GetById(Guid projectid);

        IEnumerable<EngProject> GetAllProjects();
    }
}
