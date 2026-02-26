using System.IO;

namespace FacadeMaster.Core.Configuration
{
    public class RepoOptions
    {
        public string DataDirectoryPath { get; set; } = string.Empty;
        public string ProjectsFileName { get; set; } = "projects.json";

        public string ProjectsFilePath => Path.Combine(DataDirectoryPath, ProjectsFileName);
    }
}
