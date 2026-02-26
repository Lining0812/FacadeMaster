using FacadeMaster.Core.Models;
using FacadeMaster.Core.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo _projectRepo;

        public ProjectService(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        public void CreateProject(string name,string num)
        {
            EngProject project = new EngProject
            {
                Name = name,
                Num = num
            };

            _projectRepo.Add(project);
        }
    }
}
