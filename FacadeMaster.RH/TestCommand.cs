using Rhino;
using Rhino.Commands;

namespace FacadeMaster.RH
{
    public class TestCommand : Command
    {
        public TestCommand()
        {
            Instance = this;
        }
        public static TestCommand Instance { get; private set; }

        public override string EnglishName => "FM_Test";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var window = new MainWindow();
            window.ShowDialog();
            return Result.Success;
        }
    }
}
