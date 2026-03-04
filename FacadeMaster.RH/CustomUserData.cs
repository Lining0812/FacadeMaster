using Rhino.Collections;
using Rhino.DocObjects.Custom;
using Rhino.FileIO;
using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;

namespace FacadeMaster.Shared
{
    [Guid("3B625D28-D988-4B0C-BE5B-6A416B4298E6")]
    public class CustomUserData : UserData
    {
        public string Test { get; set; } = "Test";
        public int Count { get; set; }

        public CustomUserData()
        {
        }

        public CustomUserData(string test,int count)
        {
            Test = test;
            Count = count;
        }

        public override string Description => "CustomData";

        public override string ToString()
        {
            return string.Format("Test={0},Count={1}", Test, Count);
        }

        public override bool ShouldWrite
        {
            get
            {
                if (Count == 0) return false;
                return true;
            }
        }

        protected override void OnDuplicate(UserData source)
        {
            CustomUserData data = source as CustomUserData;
            if (data != null)
            {
                Test = data.Test;
                Count = data.Count;
            }
        }

        protected override void OnTransform(Transform transform)
        {
            Test = transform.ToString();
            Count++;
            base.OnTransform(transform);
        }

        protected override bool Write(BinaryArchiveWriter archive)
        {
            archive.WriteString(Test);
            archive.WriteInt(Count);
            return true;
        }

        protected override bool Read(BinaryArchiveReader archive)
        {
            Test = archive.ReadString();
            Count = archive.ReadInt();
            return true;
        }
    }
}
