using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using System;
using System.Windows.Forms;

namespace FacadeMaster.GH.A06Brep
{
    public class ExtrudeBox : GH_Component
    {
        private bool _isExtrudeMode = true;
        public bool IsExtrudeMode
        {
            get => _isExtrudeMode;
            set
            {
                _isExtrudeMode = value;
                if (value)
                {
                    Message = "Mode1";

                }
                else
                {
                    Message = "Mode2";
                }
            }
        }

        public ExtrudeBox()
            : base("Extrude Box", "ExtrudeBox",
                  "Extrudes a box in a specified direction",
                  "FacadeMaster", "A06Brep")
        {
            Message = _isExtrudeMode ? "Mode1" : "Mode2";
        }
        public override Guid ComponentGuid => new Guid("77B831EB-D14E-4ADD-A5CC-9933DF9A32DE");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBoxParameter("Box", "B", "The box to extrude", GH_ParamAccess.item);
            pManager.AddNumberParameter("X", "X", "X direction", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "Y direction", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBoxParameter("Box", "B", "The box to extrude", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            //Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "mode1", (s, e) => { ChangeMode1(); }, true, IsExtrudeMode);
            Menu_AppendItem(menu, "mode2", (s, e) => { ChangeMode2(); }, true, !IsExtrudeMode);
            //Menu_AppendSeparator(menu);
        }

        private void ChangeMode1()
        {
            while (Params.Input.Count > 1)
            {
                Params.UnregisterInputParameter(Params.Input[1], true);
            }

            var xParam = new Param_Number();
            var yParam = new Param_Number();
            xParam.Name = "X";
            xParam.NickName = "X";
            xParam.Description = "X direction";
            xParam.Access = GH_ParamAccess.item;
            yParam.Name = "Y";
            yParam.NickName = "Y";
            yParam.Description = "Y direction";
            yParam.Access = GH_ParamAccess.item;
            Params.RegisterInputParam(xParam);
            Params.RegisterInputParam(yParam);

            IsExtrudeMode = true;
            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        private void ChangeMode2()
        {
            while (Params.Input.Count > 1)
            {
                Params.UnregisterInputParameter(Params.Input[1], true);
            }

            var xParam = new Param_Number();
            var yParam = new Param_Number();
            xParam.Name = "A";
            xParam.NickName = "A";
            xParam.Description = "A direction";
            xParam.Access = GH_ParamAccess.item;
            yParam.Name = "B";
            yParam.NickName = "B";
            yParam.Description = "B direction";
            yParam.Access = GH_ParamAccess.item;
            Params.RegisterInputParam(xParam);
            Params.RegisterInputParam(yParam);

            IsExtrudeMode = false;
            Params.OnParametersChanged();
            ExpireSolution(true);
        }


        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("IsExtrudeMode", _isExtrudeMode);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            if(reader.ItemExists("IsExtrudeMode"))
            {
                _isExtrudeMode = reader.GetBoolean("IsExtrudeMode");
                Message = _isExtrudeMode ? "Mode1" : "Mode2";
            }
            return base.Read(reader);
        }
    }
}
