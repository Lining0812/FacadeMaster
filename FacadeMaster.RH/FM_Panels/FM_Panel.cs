using Eto.Forms;
using Eto.Drawing;
using System;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_Panels
{
    [Guid("D71F0AF8-8106-46EF-A880-3F00F0A0ABB1")]
    public class FM_Panel : Panel
    {
        public FM_Panel()
        {
            Content = new Label
            {
                Text = "This is a test panel.",
                Font = new Font(SystemFont.Default, 20),
                TextColor = Colors.White,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
        }
    }
}
