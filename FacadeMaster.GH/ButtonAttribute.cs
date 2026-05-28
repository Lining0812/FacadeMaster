using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System.Drawing;
using System.Windows.Forms;

namespace FacadeMaster.GH
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonAttribute : GH_ComponentAttributes
    {
        public string DisplayText { get; set; }
        public bool Activate { get; set; }
        private Rectangle ButtonBounds { get; set; }

        public ButtonAttribute(IGH_Component component,string displayText) : base(component)
        {
            DisplayText = displayText;
            Activate = false;
        }

        protected override void Layout()
        {
            base.Layout();
            Rectangle bounds = GH_Convert.ToRectangle(this.Bounds);
            bounds.Height += 22;
            this.Bounds = bounds;

            Rectangle buttonBounds = bounds;
            buttonBounds.Y = buttonBounds.Bottom - 22;
            buttonBounds.Height = 22;
            buttonBounds.Inflate(-2, -2);
            ButtonBounds = buttonBounds;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule val = Activate ? GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Normal, DisplayText, 2, 0)
                    : GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Black, DisplayText, 2, 0);
                val.Render(graphics,Selected, Owner.Locked, false);
                val.Dispose();
            }
        }

        override public GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if(e.Button == MouseButtons.Left && ((RectangleF)ButtonBounds).Contains(e.CanvasLocation))
            {
                Activate = true;
                sender.Refresh();
                return GH_ObjectResponse.Capture;
            }
            return base.RespondToMouseDown(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (((RectangleF)ButtonBounds).Contains(e.CanvasLocation))
                {
                    Owner.ExpireSolution(true);
                    Activate = false;
                    sender.Refresh();
                }
                else
                {
                    Activate = false;
                    sender.Refresh();
                }
                return GH_ObjectResponse.Release;
            }
            return base.RespondToMouseUp(sender, e);
        }
    }
}
