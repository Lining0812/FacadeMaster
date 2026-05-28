using FacadeMaster.Core.Models;
using FacadeMaster.Core.Utilities;

namespace FacadeMaster.Core.Repos
{
    /// <summary>
    /// 工程项目仓储
    /// </summary>
    public class ProjectRepository : BaseRepo<EngProject>, IProjectRepo
    {
        public ProjectRepository(string filePath)
            : base(filePath)
        {
        }
    }
}
