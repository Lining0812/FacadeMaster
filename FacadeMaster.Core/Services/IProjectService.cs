using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Models;
using System;
using System.Collections.Generic;

namespace FacadeMaster.Core.Services
{
    public interface IProjectService
    {
        EngProject CreateProject(CreateProjectRequest dto);

        ProjectBatch AddBatchToProject(Guid id, CreateBatchRequest dto);

        ICollection<ProjectBatch> GetBatchesByProject(EngProject p);
    }
}
