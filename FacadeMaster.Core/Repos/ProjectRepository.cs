using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Models;
using System;

namespace FacadeMaster.Core.Repos
{
    public class ProjectRepository : BaseRepo<EngProject>, IProjectRepo
    {

        protected override string FilePath => "C:\\Users\\Administrator\\Desktop\\Projects.Json";
    }
}
