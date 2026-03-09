using FacadeMaster.Core.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacadeMaster.RH.FM_UserData
{
    /// <summary>
    /// 玻璃加工件 - 待完善
    /// </summary>
    public class GlassJGJ : JGJData
    {
        public new FM_Glass Material { get; set; }

        public override string Description => "玻璃加工件";
    }
}
