using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Mappers;
using FacadeMaster.Core.Models;
using FacadeMaster.Core.Repos;
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

        public ICollection<ProjectBatch> GetBatchesByProject(EngProject p)
        {
            return p.ProjectBatchs;
        }
    }
}
