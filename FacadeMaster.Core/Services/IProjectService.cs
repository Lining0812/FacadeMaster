using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.Core.Services
{
    public interface IProjectService
    {
        void CreateProject(string name, string num);
    }
}
