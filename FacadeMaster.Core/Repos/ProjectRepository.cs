using FacadeMaster.Core.Models;
using FacadeMaster.Core.Utilities;
using System.Collections.Generic;

namespace FacadeMaster.Core.Repos
{
    public class ProjectRepository : BaseRepo<EngProject>, IProjectRepo
    {

        //private readonly string _filePath = "C:\\Users\\Administrator\\Desktop\\Project.Json";
        private readonly string _filePath = "D:\\Project.Json";

        public ProjectRepository()
        {
            if (JsonHelper.TryDeserializeFromFile(_filePath, out ICollection<EngProject> loadeds))
            {
                foreach (var project in loadeds)
                {
                    Entities.Add(project);
                }
            }
        }

        public override void SaveChanges()
        {
            JsonHelper.SerializeToFile(Entities, _filePath);
        }
    }
}
