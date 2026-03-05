using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Models;
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
        /// <returns></returns>
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
    }
}
