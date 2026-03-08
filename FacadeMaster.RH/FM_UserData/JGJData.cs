using Rhino;
using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_UserData
{
    [Guid("AED87E30-EA39-48DA-9242-59886E3F2FDD")]
    public class JGJData : UserData
    {
        public Plane BH_Plane { get; set; } = Plane.WorldXY;
        public Plane CT_Plane { get; set; } = Plane.WorldXY;

        public override string Description => "JGJData";

        public override string ToString()
        {
            return string.Format("编号平面={0},出图平面={1}", BH_Plane, CT_Plane);
        }

        protected override void OnTransform(Transform transform)
        {
            Plane bhPlane = BH_Plane;
            Plane ctPlane = CT_Plane;
            if (bhPlane.Transform(transform) && ctPlane.Transform(transform))
            {
                BH_Plane = bhPlane;
                CT_Plane = ctPlane;
                RhinoApp.WriteLine($"变换成功:{BH_Plane.Origin},{CT_Plane.Origin}");
            }
            base.OnTransform(transform);
        }
    }
}
