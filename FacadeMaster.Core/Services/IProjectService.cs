using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Models;
using System.Collections.Generic;

namespace FacadeMaster.Core.Services
{
    public interface IProjectService
    {
        EngProject CreateProject(CreateProjectRequest dto);

        ICollection<ProjectBatch> GetBatchesByProject(EngProject p);
    }
}
