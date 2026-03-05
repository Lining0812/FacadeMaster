using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace FacadeMaster.GH
{
    public class XXComponent : GH_Component
    {
        public XXComponent()
          : base("GHComponent", "CP",
            "Create Project",
            "FacadeMater", "Primitive")
        {
        }
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("cefbceb0-ca51-4644-be6a-3c4523fc8ee4");

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("i", "i", "i", GH_ParamAccess.item);
            pManager.AddTextParameter("c", "c", "c", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("b", "b", "b", GH_ParamAccess.item);
            pManager.AddTextParameter("i", "i", "i", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var title = "";
            var code = "";

            if (!DA.GetData(0, ref title) || !DA.GetData(1, ref code)) return;

            var dto = new CreateProjectRequest(title, code);

            ProjectService service = new ProjectService(new ProjectRepository());

            var project = service.CreateProject(dto);

            if (project == null)
            {
                DA.SetData(0, false);
                DA.SetData(1, "添加失败");
            }
            DA.SetData(0,true); 
            DA.SetData(1, project.Name);

            this.Attributes
        }
    }

    public class GHAttributes : GH_ComponentAttributes
    {
        public GHAttributes(GH_Component owner) : base(owner)
        {
        }
    }
}