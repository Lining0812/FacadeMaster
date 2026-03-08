using FacadeMaster.Core.DTOs;
using FacadeMaster.Core.Repos;
using FacadeMaster.Core.Services;
using FacadeMaster.RH;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

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
            pManager.AddBooleanParameter("b", "b", "b", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("p", "p", "p", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;

            if (!DA.GetData(0, ref run)) return;

            if (!run) return;
            var projects = RhinoPlugin.Instance.GetService<IProjectService>().GetAllProjects();

            DA.SetDataList(0, projects.Select(p => $"{p.Name} - {p.Num}"));
        }
    }

}