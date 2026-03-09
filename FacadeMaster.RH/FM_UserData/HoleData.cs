using Rhino.Collections;
using Rhino.DocObjects.Custom;
using Rhino.FileIO;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FacadeMaster.RH.FM_UserData
{
    [Guid("3B625D28-D988-4B0C-BE5B-6A416B4298E6")]
    public class HoleData : UserData
    {
        public string Test { get; set; } = "Test";
        public IEnumerable<Brep> Hole { get; set; } = null;

        public override string Description => "HoleData";

        public override string ToString()
        {
            return string.Format("Test={0}", Test);
        }

        public override bool ShouldWrite => true;

        protected override void OnDuplicate(UserData source)
        {
            HoleData data = source as HoleData;
            if (data != null)
            {
                Test = data.Test;
            }
        }

        protected override void OnTransform(Transform transform)
        {
            base.OnTransform(transform);
        }

        protected override bool Write(BinaryArchiveWriter archive)
        {
            var dic = new ArchivableDictionary();
            dic.Set("Hole", Hole);
            archive.WriteDictionary(dic);
            return true;
        }

        protected override bool Read(BinaryArchiveReader archive)
        {
            return true;
        }
    }
}
