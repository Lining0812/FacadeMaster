using Eto.Forms;
using FacadeMaster.Core.Models.Material;
using FacadeMaster.Core.Services;
using FacadeMaster.RH.FM_Panels;
using Microsoft.Extensions.DependencyInjection;
using Rhino;
using Rhino.Commands;

namespace FacadeMaster.RH.FM_Command
{
    public class AddMaterialCommand : Rhino.Commands.Command
    {
        private IMaterialService _services;
        private IMaterialService Services =>
            _services = RhinoPlugin.Instance?.ServiceProvider?.GetRequiredService<IMaterialService>();

        public static AddMaterialCommand Instance { get; private set; }
        public AddMaterialCommand()
        {
            Instance = this;
        }
        public override string EnglishName => "AddMaterial";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (Services == null)
            {
                RhinoApp.WriteLine("Plugin services are not initialized.");
                return Result.Failure;
            }

            var testMaterial = new FM_AluPlate("测试数据", "测试数据", "测试数据", 3.0, "测试数据");
            Services.Add(testMaterial);

            MessageBox.Show("创建物料成功", MessageBoxButtons.OK, MessageBoxType.Information);

            return Result.Success;
        }
    }
}
