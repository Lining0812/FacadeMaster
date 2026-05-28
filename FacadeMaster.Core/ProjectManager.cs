using FacadeMaster.Core.Models;
using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacadeMaster.Core
{
    public class ProjectManager
    {
        private readonly IProjectService _projectService;
        public List<EngProject> AllProjects { get; private set; } = new List<EngProject>();
        private EngProject _activeProject;
        public EngProject ActiveProject
        {
            get => _activeProject;
            set
            {
                if(ReferenceEquals(_activeProject, value)) return;
                _activeProject = value;
                ActiveProjectChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>项目列表发生变更：新增、删除项目时触发</summary>
        public event EventHandler ProjectChanged;
        /// <summary>激活项目切换事件</summary>
        public event EventHandler ActiveProjectChanged;
        /// <summary>当前项目下批次/物料数据变更事件（新增、编辑、删除批次物料）</summary>
        public event EventHandler ProjectContentChanged;

        public ProjectManager(IProjectService service)
        {
            _projectService = service;
            ReloadProjects();
            ActiveProject = AllProjects.FirstOrDefault();
        }

        /// <summary>
        /// 从数据库重新加载全部项目，更新缓存，触发通知
        /// </summary>
        /// <param name="activeProjectId">
        /// 传入时按 Id 恢复激活项目（换库后使用）；找不到则退回第一个项目。
        /// 传 null 时保持当前激活项目不变。
        /// </param>
        public void ReloadProjects(Guid? activeProjectId = null)
        {
            AllProjects = _projectService?.GetAllProjects()?.ToList() ?? new List<EngProject>();

            if (activeProjectId.HasValue)
            {
                ActiveProject = AllProjects.FirstOrDefault(p => p.Id == activeProjectId.Value)
                                ?? AllProjects.FirstOrDefault();
            }

            ProjectChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>通知：当前项目内部数据（批次/物料）变化</summary>
        public void NotifyProjectContentUpdated()
        {
            ProjectContentChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 创建工程项目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public EngProject CreateProject(string name,string code)
        {
            var proj = _projectService.CreateProject(name, code);
            ReloadProjects();
            ActiveProject = proj;
            return proj;
        }

        public void DeleteProject(EngProject project)
        {
            _projectService.DeleteProject(project);
            ReloadProjects();
            // 删除后自动切换到第一个或者null
            ActiveProject = AllProjects.FirstOrDefault();
        }

        // ---------- 批次操作 ----------
        public List<ProjectBatch> GetBatches(EngProject project)
        {
            var batches = project?.ProjectBatchs?.ToList() ?? new List<ProjectBatch>();
            return batches.OrderBy(b => b.WBS, StringComparer.OrdinalIgnoreCase)
                .ThenBy(b => b.ProcessNum, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        // ---------- 物料操作 ----------
        public List<FM_Material> GetMaterials(EngProject project)
            => project == null
                ? new List<FM_Material>()
                : _projectService.GetProjectMaterials(project.Id).ToList();

        /// <summary>
        /// 暴露 service 供创建/编辑对话框使用（如有需要）。
        /// 若想彻底解耦，可再封装 CreateBatch / CreateMaterial 等方法。
        /// </summary>
        public IProjectService Service => _projectService;
    }
}
