using Eto.Drawing;
using FacadeMaster.Core.Models;
using FacadeMaster.UI.Dialogs;
using Rhino;
using Rhino.Commands;
using Rhino.UI;


namespace FacadeMaster.RH.FM_Command
{
    public class TestShowDialogCommand : Command
    {
        public override string EnglishName => "ShowFm_Dialog";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var owner = RhinoEtoApp.MainWindowForDocument(doc);
            var ownerBounds = owner.Bounds;

            //var project = new EngProject("测试", "测试");

            //var dialog = new FM_Dialog();
            ////dialog.Width = 1200;
            ////dialog.Height = 570;
            //dialog.Location = new Point(
            //    ownerBounds.X + (ownerBounds.Width - dialog.Width) / 2,
            //    ownerBounds.Y + (ownerBounds.Height - dialog.Height) / 2
            //    );

            //dialog.ShowModal();
            return Result.Success;
        }
    }
}
