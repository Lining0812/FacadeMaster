using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Mappers;
using FacadeMaster.Core.Models;
using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Repos;
using System;
using System.Collections.Generic;

namespace FacadeMaster.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _projectRepo;

        public ProjectService(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public EngProject CreateProject(CreateProjectRequest dto)
        {
            var project = ProjectMapper.ToEntity(dto);

            _projectRepo.Add(project);

            return project;
        }

        public ProjectBatch AddBatchToProject(Guid projectid, CreateBatchRequest dto)
        {
            var project = _projectRepo.GetById(projectid);
            if (project == null)
                throw new InvalidOperationException($"项目 {project.Id} 不存在");

            var batch = BatchMapper.ToEntity(dto);
            project.ProjectBatchs.Add(batch);
            _projectRepo.Update(project);

            return batch;
        }

        public ICollection<ProjectBatch> GetBatchesByProject(EngProject p)
        {
            return p.ProjectBatchs;
        }

        public void AddMaterialToProject(Guid projectid, FM_BaseMaterial material)
        {
            var project = _projectRepo.GetById(projectid);
            if (project == null)
                throw new InvalidOperationException($"项目 {project.Id} 不存在");

            project.Materials.Add(material);
            _projectRepo.Update(project);
        }

        public EngProject GetById(Guid projectid)
        {
             return _projectRepo.GetById(projectid);
        }
    }
}
