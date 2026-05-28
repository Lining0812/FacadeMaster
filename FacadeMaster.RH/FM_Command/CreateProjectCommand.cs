using FacadeMaster.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;

namespace FacadeMaster.RH.FM_Command
{
    public class CreateProjectCommand : Command
    {
        private IProjectService _services;
        private IProjectService Services =>
            _services = RhinoPlugin.Instance?.ServiceProvider?.GetRequiredService<IProjectService>();

        public static CreateProjectCommand Instance { get; private set; }
        public CreateProjectCommand()
        {
            Instance = this;
        }
        public override string EnglishName => "CreateProject";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (Services == null)
            {
                RhinoApp.WriteLine("Plugin services are not initialized.");
                return Result.Failure;
            }

            RhinoApp.WriteLine("The {0} command will add a project right now.", EnglishName);

            string projectName;
            using(var getStringAction = new GetString())
            {
                getStringAction.SetCommandPrompt("Please enter the project name");
                if (getStringAction.Get() != GetResult.String)
                {
                    RhinoApp.WriteLine("No project name was entered.");
                    return getStringAction.CommandResult();
                }
                projectName = getStringAction.StringResult();
            }

            string projectCode;
            using (var getStringAction = new GetString())
            {
                getStringAction.SetCommandPrompt("Please enter the project code");
                if (getStringAction.Get() != GetResult.String)
                {
                    RhinoApp.WriteLine("No project code was entered.");
                    return getStringAction.CommandResult();
                }
                projectCode = getStringAction.StringResult();
            }

            Services.CreateProject(projectName, projectCode);

            return Result.Success;
        }
    }
}
