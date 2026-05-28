using FacadeMaster.Core.Models;
using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacadeMaster.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IMaterialService _materialService;

        public ProjectService(IProjectRepo projectRepo, IMaterialService materialService)
        {
            _projectRepo = projectRepo;
            _materialService = materialService;
        }

        public EngProject CreateProject(string name, string code)
        {
            var project = new EngProject(name, code);

            _projectRepo.Add(project);

            return project;
        }
        public void DeleteProject(EngProject project)
        {
            _projectRepo.Delete(project);
        }

        public EngProject GetById(Guid projectid)
        {
            return _projectRepo.GetById(projectid);
        }

        public IEnumerable<EngProject> GetAllProjects()
        {
            return _projectRepo.GetAll();
        }

        public void AddBatchToProject(Guid projectId, string name, string wbs, string processNum)
        {
            var project = GetById(projectId);
            var batch = project.AddBatch(name, wbs, processNum);
            _projectRepo.Update(project);
        }

        public void RemoveBatchFromProject(Guid projectId, Guid batchId)
        {
            var project = GetById(projectId);
            project.RemoveBatch(batchId);
            _projectRepo.Update(project);
        }

        public void UpdateBatch(Guid projectId, Guid batchId, string name, string wbs, string processNum)
        {
            var project = GetById(projectId);
            var batch = project.GetBatch(batchId);
            if(batch==null) throw new InvalidOperationException($"批次不存在");
            batch.UpdateInfo(name, wbs, processNum);
            _projectRepo.Update(project);
        }

        public void AddMaterialToProject(Guid projectId, Guid materialId)
        {
            var project = GetById(projectId);
            if (project == null) throw new InvalidOperationException("项目不存在");

            // 校验全局库中是否存在且有效
            var material = _materialService.GetById(materialId);
            if (material == null) throw new InvalidOperationException("物料不存在或已删除");

            if (!project.Materials.Contains(materialId))
            {
                project.Materials.Add(materialId);
                _projectRepo.Update(project);  // 这里要求 Update 是属性拷贝，见下文
            }
        }

        public void RemoveMaterialFromProject(Guid projectId, Guid materialId)
        {
            var project = GetById(projectId);
            if (project == null) return;

            project.Materials.Remove(materialId);
            _projectRepo.Update(project);
        }

        public IEnumerable<FM_Material> GetProjectMaterials(Guid projectId)
        {
            var project = GetById(projectId);
            if (project == null) return Enumerable.Empty<FM_Material>();

            return project.Materials
                .Select(id => _materialService.GetById(id))
                .Where(m => m != null);  // 自动过滤已删除的物料
        }
    }
}
