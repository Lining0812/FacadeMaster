using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Rhino;
using Rhino.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.RH.FM_Command
{
    public class GetAllMaterialCommand : Command
    {
        private IMaterialService _services;
        private IMaterialService Services =>
            _services = RhinoPlugin.Instance?.ServiceProvider?.GetRequiredService<IMaterialService>();
        public override string EnglishName => "GetAllMaterial";
        public static GetAllMaterialCommand Instance { get; private set; }
        public GetAllMaterialCommand()
        {
            Instance = this;
        }
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (Services == null)
            {
                RhinoApp.WriteLine("Plugin services are not initialized.");
                return Result.Failure;
            }
            var materials = Services.GetAll();
            foreach ( var material in materials )
            {
                RhinoApp.WriteLine($"{material.M_Name} | {material.M_Code}");
            }

            return Result.Success;
        }
    }
}
